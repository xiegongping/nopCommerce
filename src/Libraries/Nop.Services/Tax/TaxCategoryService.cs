﻿using System;
using System.Collections.Generic;
using System.Linq;
using Nop.Core.Domain.Tax;
using Nop.Data;
using Nop.Services.Caching.CachingDefaults;
using Nop.Services.Caching.Extensions;
using Nop.Services.Events;

namespace Nop.Services.Tax
{
    /// <summary>
    /// Tax category service
    /// </summary>
    public partial class TaxCategoryService : ITaxCategoryService
    {
        #region Fields

        private readonly IEventPublisher _eventPublisher;
        private readonly IRepository<TaxCategory> _taxCategoryRepository;

        #endregion

        #region Ctor

        public TaxCategoryService(IEventPublisher eventPublisher,
            IRepository<TaxCategory> taxCategoryRepository)
        {
            _eventPublisher = eventPublisher;
            _taxCategoryRepository = taxCategoryRepository;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Deletes a tax category
        /// </summary>
        /// <param name="taxCategory">Tax category</param>
        public virtual void DeleteTaxCategory(TaxCategory taxCategory)
        {
            if (taxCategory == null)
                throw new ArgumentNullException(nameof(taxCategory));

            _taxCategoryRepository.Delete(taxCategory);

            //event notification
            _eventPublisher.EntityDeleted(taxCategory);
        }

        /// <summary>
        /// Delete tax categories
        /// </summary>
        /// <param name="taxCategories">Tax categories</param>
        public virtual void DeleteTaxCategories(IList<TaxCategory> taxCategories)
        {
            if (taxCategories == null)
                throw new ArgumentNullException(nameof(taxCategories));

            foreach (var taxCategory in taxCategories)
            {
                DeleteTaxCategory(taxCategory);
            }
        }

        /// <summary>
        /// Gets all tax categories
        /// </summary>
        /// <returns>Tax categories</returns>
        public virtual IList<TaxCategory> GetAllTaxCategories()
        {
            var query = from tc in _taxCategoryRepository.Table
                orderby tc.DisplayOrder, tc.Id
                select tc;

            var taxCategories = query.ToCachedList(NopTaxCachingDefaults.TaxCategoriesAllCacheKey);

            return taxCategories;
        }

        /// <summary>
        /// Gets a tax category
        /// </summary>
        /// <param name="taxCategoryId">Tax category identifier</param>
        /// <returns>Tax category</returns>
        public virtual TaxCategory GetTaxCategoryById(int taxCategoryId)
        {
            if (taxCategoryId == 0)
                return null;

            return _taxCategoryRepository.ToCachedGetById(taxCategoryId);
        }

        /// <summary>
        /// Get tax categories
        /// </summary>
        /// <param name="taxCategoriesIds">Tax categories identifiers</param>
        /// <returns>Tax categories</returns>
        public virtual IList<TaxCategory> GetTaxCategoriesByIds(int[] taxCategoriesIds)
        {
            if (taxCategoriesIds == null || taxCategoriesIds.Length == 0)
                return new List<TaxCategory>();

            var query = from t in _taxCategoryRepository.Table
                        where taxCategoriesIds.Contains(t.Id)
                        select t;

            return query.ToList();
        }

        /// <summary>
        /// Inserts a tax category
        /// </summary>
        /// <param name="taxCategory">Tax category</param>
        public virtual void InsertTaxCategory(TaxCategory taxCategory)
        {
            if (taxCategory == null)
                throw new ArgumentNullException(nameof(taxCategory));

            _taxCategoryRepository.Insert(taxCategory);

            //event notification
            _eventPublisher.EntityInserted(taxCategory);
        }

        /// <summary>
        /// Updates the tax category
        /// </summary>
        /// <param name="taxCategory">Tax category</param>
        public virtual void UpdateTaxCategory(TaxCategory taxCategory)
        {
            if (taxCategory == null)
                throw new ArgumentNullException(nameof(taxCategory));

            _taxCategoryRepository.Update(taxCategory);

            //event notification
            _eventPublisher.EntityUpdated(taxCategory);
        }

        #endregion
    }
}