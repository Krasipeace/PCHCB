﻿namespace PCHCB.Web.ViewModels.Cooler
{
    /// <summary>
    /// Short info about Cooler
    /// </summary>
    public class CoolerAllViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public decimal Price { get; set; }

        public string ImageUrl { get; set; } = null!;
    }
}
