using FreakyFashion.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Umbraco.Cms.Core.Cache;
using Umbraco.Cms.Core.Logging;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Routing;
using Umbraco.Cms.Core.Scoping;
using Umbraco.Cms.Core.Security;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Infrastructure.Persistence;
using Umbraco.Cms.Web.Common.Filters;
using Umbraco.Cms.Web.Common.Security;
using Umbraco.Cms.Web.Website.Controllers;
using Umbraco.Cms.Web.Website.Models;

namespace FreakyFashion.Controllers;

public class CheckoutFormController : SurfaceController
{
    private readonly IMemberManager _memberManager;
    private readonly IMemberService _memberService;
    private readonly IMemberSignInManager _memberSignInManager;
    private readonly ICoreScopeProvider _scopeProvider;

    public CheckoutFormController(
        IMemberManager memberManager,
        IMemberService memberService,
        IUmbracoContextAccessor umbracoContextAccessor,
        IUmbracoDatabaseFactory databaseFactory,
        ServiceContext services,
        AppCaches appCaches,
        IProfilingLogger profilingLogger,
        IPublishedUrlProvider publishedUrlProvider,
        IMemberSignInManager memberSignInManager,
        ICoreScopeProvider scopeProvider)
        : base(umbracoContextAccessor, databaseFactory, services, appCaches, profilingLogger, publishedUrlProvider)
    {
        _memberManager = memberManager;
        _memberService = memberService;
        _memberSignInManager = memberSignInManager;
        _scopeProvider = scopeProvider;
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    [ValidateUmbracoFormRouteString]
    public async Task<IActionResult> HandleRegisterMember(RegisterViewModel model)
    {
        if (ModelState.IsValid == false)
        {
            return CurrentUmbracoPage();
        }
    
        IdentityResult result = await RegisterMemberAsync(model);
        if (result.Succeeded)
        {
            TempData["FormSuccess"] = true;
    
            // Redirect to current page by default.
            return RedirectToCurrentUmbracoPage();
        }
    
        AddErrors(result);
        return CurrentUmbracoPage();
    }
    
    private void AddErrors(IdentityResult result)
    {
        foreach (IdentityError? error in result.Errors)
        {
            ModelState.AddModelError("registerViewModel", error.Description);
        }
    }
    
    private async Task<IdentityResult> RegisterMemberAsync(RegisterViewModel model, bool logMemberIn = true)
    {
        using ICoreScope scope = _scopeProvider.CreateCoreScope(autoComplete: true);

        var name = $"{model.FirstName} {model.LastName}";

        var identityUser =
            MemberIdentityUser.CreateNew(model.Email, model.Email, "Member", true, name);
        IdentityResult identityResult = await _memberManager.CreateAsync(
            identityUser,
            model.Password);
    
        if (identityResult.Succeeded)
        {
            // Update the custom properties
            // TODO: See TODO in MembersIdentityUser, Should we support custom member properties for persistence/retrieval?
            IMember? member = _memberService.GetByKey(identityUser.Key);
            if (member == null)
            {
                // should never happen
                throw new InvalidOperationException($"Could not find a member with key: {member?.Key}.");
            }
    
            _memberService.Save(member);
    
            if (logMemberIn)
            {
                await _memberSignInManager.SignInAsync(identityUser, false);
            }
        }
    
        return identityResult;
    }

} 
