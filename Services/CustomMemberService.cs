using Microsoft.AspNetCore.Identity;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Scoping;
using Umbraco.Cms.Core.Security;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Web.Common.Security;

namespace FreakyFashion.Services;

public interface ICustomMemberService
{
    IMember? GetLoggedInMember();

    bool IsLoggedIn();

    Task<IdentityResult> RegisterMemberAsync(string email, string firstName, string lastName, string password, string memberTypeAlias = "Member", bool logMemberIn = true);
}

public class CustomMemberService : ICustomMemberService
{
    private readonly IMemberManager _memberManager;
    private readonly IMemberService _memberService;
    private readonly IMemberSignInManager _memberSignInManager;
    private readonly ICoreScopeProvider _scopeProvider;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CustomMemberService(ICoreScopeProvider scopeProvider,
        IMemberSignInManager memberSignInManager,
        IMemberService memberService,
        IMemberManager memberManager,
        IHttpContextAccessor httpContextAccessor)
    {
        _scopeProvider = scopeProvider;
        _memberSignInManager = memberSignInManager;
        _memberService = memberService;
        _memberManager = memberManager;
        _httpContextAccessor = httpContextAccessor;
    }

    public bool IsLoggedIn()
    {
        return _httpContextAccessor.HttpContext.User.Identity.IsAuthenticated;
    }

    public IMember? GetLoggedInMember()
    {
        if (!IsLoggedIn()) return null;
        return _memberService.GetById(Int32.Parse(_httpContextAccessor.HttpContext.User.Identity.GetUserId()));
    }

    public async Task<IdentityResult> RegisterMemberAsync(string email,
                                                          string firstName,
                                                          string lastName,
                                                          string password,
                                                          string memberTypeAlias = "Member",
                                                          bool logMemberIn = true)
    {
        using ICoreScope scope = _scopeProvider.CreateCoreScope(autoComplete: true);

        var name = $"{firstName} {lastName}";

        var identityUser =
            MemberIdentityUser.CreateNew(email, email, memberTypeAlias, true, name);
        IdentityResult identityResult = await _memberManager.CreateAsync(
            identityUser,
            password);

        if (identityResult.Succeeded)
        {
            IMember? member = _memberService.GetByKey(identityUser.Key);
            if (member == null)
            {
                throw new InvalidOperationException($"Could not find a member with key: {member?.Key}.");
            }

            _memberService.Save(member);
            _memberService.AssignRole(member.Id, "Customer");

            if (logMemberIn)
            {
                await _memberSignInManager.SignInAsync(identityUser, false);
            }
        }

        return identityResult;
    }
}