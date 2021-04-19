using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace QueryFilterServiceApp.Models {
    public class LoginScreenModel {
        public string UserId { get; set; }
        public IList<SelectListItem> Users { get; set; }
    }
}
