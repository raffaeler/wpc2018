using System;
using System.Collections.Generic;
using System.Text;

namespace ModelLibrary
{
    public enum ArticleState
    {
        /// <summary>
        /// Only visible internally from the sellers and admins
        /// </summary>
        Internal,

        /// <summary>
        /// Visible to everyone
        /// </summary>
        ListedForSelling,

        /// <summary>
        /// Only visible from the seller and the buyer
        /// </summary>
        Sold,

        /// <summary>
        /// Only visible from the seller and the buyer
        /// </summary>
        Returned,
    }
}
