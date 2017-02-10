using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aci.X.IwsLib.Storefront
{
    public class ItemPromotion
    {
        //public ItemPromotionStatus status;
        public string productOfferingId;
        public PromotionDetails promotionDetails;
        public string status;
    }

    //public class ItemPromotionStatus
    //{

    //}

    public class PromotionDetails
    {
        public string headline;
        public string subHeadline;
        public string title;
    }
}
