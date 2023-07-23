﻿namespace PCHCB.Web.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using PCHCB.Services.Data.Contracts;
    using PCHCB.Web.Infrastructure.Extensions;
    using PCHCB.Web.ViewModels.Storage;

    using static PCHCB.Common.NotificationMessages;
    using static PCHCB.Common.ErrorMessages.Provider;
    using static PCHCB.Common.ErrorMessages.Storage;
    using static PCHCB.Common.SuccessMessages;
    using static PCHCB.Common.ExceptionMessages;

    [Authorize]
    public class StorageController : Controller
    {
        private readonly IStorageService storageService;
        private readonly IProviderService providerService;

        public StorageController(IStorageService storageService, IProviderService providerService)
        {
            this.storageService = storageService;
            this.providerService = providerService;
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            bool isProvider = await this.providerService
                .ProviderExistsByUserIdAsync(this.User.GetId()!);

            if (!isProvider)
            {
                this.TempData[ErrorMessage] = UserCannotAddStorageDevicesErrorMessage;

                return this.RedirectToAction("BecomeProvider", "Provider");
            }

            StorageFormModel model = new StorageFormModel();

            return this.View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Add(StorageFormModel model)
        {
            bool isProvider = await this.providerService
                .ProviderExistsByUserIdAsync(this.User.GetId()!);

            if (!isProvider)
            {
                this.TempData[ErrorMessage] = UserCannotAddStorageDevicesErrorMessage;

                return this.RedirectToAction("BecomeProvider", "Provider");
            }

            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }

            try
            {
                string? providerId = await this.providerService
                .GetProviderByUserIdAsync(this.User.GetId()!);

                int storageId = await this.storageService.CreateStorageAsync(providerId!, model);

                this.TempData[SuccessMessage] = StorageAddedSuccessfully;

                return this.RedirectToAction("Details", "Storage");
            }
            catch (Exception)
            {
                this.ModelState.AddModelError(string.Empty, GeneralErrorMessage);

                return this.View(model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            bool storageExists = await storageService
                .IsStorageExistByIdAsync(id);
            if (!storageExists)
            {
                TempData[ErrorMessage] = StorageWithIdDoesNotExist;

                return RedirectToAction("All", "Components");
            }

            bool isUserProvider = await providerService
                .ProviderExistsByUserIdAsync(User.GetId()!);
            if (!isUserProvider)
            {
                TempData[ErrorMessage] = UserCannotEditStorageDevicesErrorMessage;

                return RedirectToAction("BecomeProvider", "Provider");
            }
            string? providerId =
                await providerService.GetProviderByUserIdAsync(User.GetId()!);
            bool isProviderOwner = await storageService
                .IsProviderIdOwnerOfStorageIdAsync(providerId!, id);

            if (!isProviderOwner)
            {
                TempData[ErrorMessage] = ProviderCannotEditStorageDeviceHeDoesNotOwnErrorMessage;

                return RedirectToAction("Mine", "Provider");
            }

            try
            {
                StorageFormModel formModel = await storageService
                    .GetStorageForEditByIdAsync(id);

                return View(formModel);
            }
            catch (Exception)
            {
                return GeneralError();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, StorageFormModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            bool storageExists = await storageService
                .IsStorageExistByIdAsync(id);
            if (!storageExists)
            {
                TempData[ErrorMessage] = StorageWithIdDoesNotExist;

                return RedirectToAction("All", "Components");
            }

            bool isUserProvider = await providerService
                .ProviderExistsByUserIdAsync(User.GetId()!);
            if (!isUserProvider)
            {
                TempData[ErrorMessage] = UserCannotEditStorageDevicesErrorMessage;

                return RedirectToAction("BecomeProvider", "Provider");
            }

            string? providerId =
                await providerService.GetProviderByUserIdAsync(User.GetId()!);
            bool isProviderOwner = await storageService
                .IsProviderIdOwnerOfStorageIdAsync(providerId!, id);

            if (!isProviderOwner)
            {
                TempData[ErrorMessage] = ProviderCannotEditStorageDeviceHeDoesNotOwnErrorMessage;

                return RedirectToAction("Mine", "Provider");
            }

            try
            {
                await storageService.EditStorageByIdAndFormModelAsync(id, model);
            }
            catch (Exception)
            {
                ModelState.AddModelError(string.Empty,
                    GeneralErrorMessage);

                return View(model);
            }

            TempData[SuccessMessage] = StorageEditedSuccessfully;

            return RedirectToAction("Details", "Storage", new { id });
        }

        private IActionResult GeneralError()
        {
            this.TempData[ErrorMessage] = GeneralErrorMessage;

            return this.RedirectToAction("Index", "Home");
        }
    }
}