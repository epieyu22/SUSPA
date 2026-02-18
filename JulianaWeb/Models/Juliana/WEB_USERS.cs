namespace JulianaWeb.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Text;

    public partial class WEB_USERS
    {
        [Key]
        public int Cod_Web_Users { get; set; }

        [StringLength(15)]
        public string Cedula { get; set; }

        [StringLength(2)]
        public string Estado { get; set; }

        [StringLength(15)]
        public string DB_Name { get; set; }

        public string generaateNewPass()
        {
            int length = 8;
            const string valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890$%&#@";
            StringBuilder res = new StringBuilder();
            Random rnd = new Random();
            while (0 < length--)
            {
                res.Append(valid[rnd.Next(valid.Length)]);
            }
            return res.ToString();
        }
    }
}
