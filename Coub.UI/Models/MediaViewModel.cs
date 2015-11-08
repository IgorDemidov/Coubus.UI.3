using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Adam.Core.Orders;
using Adam.Core.Records;

namespace Coub.UI.Models
{
    public class MediaViewModel
    {
        public Guid Id { get; set; }
        public string Author { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }

        public MediaViewModel(Record record)
        {
            Id = record.Id;
            Author = record.Fields["Author"].ToString();
            Description = record.Fields["Description"].ToString();
            Name = record.Fields["Name"].ToString();
        }
    }
}