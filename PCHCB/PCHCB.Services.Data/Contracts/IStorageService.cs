﻿namespace PCHCB.Services.Data.Contracts
{
    using PCHCB.Web.ViewModels.Storage;

    public interface IStorageService
    {
        Task CreateStorage(string providerId, StorageFormModel model);

        Task EditStorageByIdAndFormModelAsync(int storageId, StorageFormModel model);

        Task DeleteStorageByIdAsync(int storageId);

        Task<bool> IsStorageExistByIdAsync(int storageId);

        Task<bool> IsProviderIdOwnerOfStorageIdAsync(string providerId, int storageId);

        //Task<StorageDeleteDetailsViewModel> GetStorageForDeleteByIdAsync(int storageId);

        //Task<StorageDetailsViewModel> GetStorageDetailsAsync(int storageId);

        //Task<IEnumerable<StorageAllViewModel>> AllByProviderIdAsync(string providerId);

        //Task<IEnumerable<StorageAllViewModel>> AllAvailableStorages(int storageId);
    }
}
