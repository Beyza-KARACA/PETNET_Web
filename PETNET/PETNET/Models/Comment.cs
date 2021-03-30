namespace PETNET.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Comment
    {
        public int CommentID { get; set; }

        [StringLength(200)]
        public string Issue { get; set; }

        public int? UserID { get; set; }

        public int? BlogID { get; set; }

        [StringLength(10)]
        public string Date { get; set; }

        public virtual Blog Blog { get; set; }

        public virtual User User { get; set; }
    }
}
