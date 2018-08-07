using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DigitalSignage.Domain
{
    public class PlayerViewModel
    {
        public int PlayerId { get; set; }
        public string PlayerSerialNo { get; set; }
        public string PlayerName { get; set; }
        public bool IsActive { get; set; }
        public int CreatedBy { get; set; }
        public int UpdatedBy { get; set; }
        public int GroupId { get; set; }
        public int AccountID { get; set; }
    }
}