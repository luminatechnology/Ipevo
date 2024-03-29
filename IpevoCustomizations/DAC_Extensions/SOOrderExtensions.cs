﻿using System;
using PX.Data;
using PX.Objects.CM;

namespace PX.Objects.SO
{
    public class SOOrderExt : PXCacheExtension<PX.Objects.SO.SOOrder>
    {
        // Jira IP-13
        #region UsrSubTot
        [PXDecimal()]
        [PXDefault(TypeCode.Decimal, "0.0", PersistingCheck = PXPersistingCheck.Nothing)]
        public virtual decimal? UsrSubTot { get; set; }
        public abstract class usrSubTot : PX.Data.BQL.BqlDecimal.Field<usrSubTot> { }
        #endregion

        #region UsrCurySubTot
        [PXCurrency(typeof(SOOrder.curyInfoID), typeof(usrSubTot))]
        [PXUIField(DisplayName = "Sub Total", IsReadOnly = true)]
        [PXFormula(typeof(Add<SOOrder.curyMiscTot, SOOrder.curyLineTotal>))]
        public virtual decimal? UsrCurySubTot { get; set; }
        public abstract class usrCurySubTot : PX.Data.BQL.BqlDecimal.Field<usrCurySubTot> { }
        #endregion

        #region UsrRemainingCreditLimit
        [PXBaseCury()]
        [PXUIField(DisplayName = "Remaining Credit Limit", IsReadOnly = true, Enabled = false)]
        public virtual Decimal? UsrRemainingCreditLimit { get; set; }
        public abstract class usrRemainingCreditLimit : PX.Data.BQL.BqlDecimal.Field<usrRemainingCreditLimit> { }
        #endregion
    }
}
