using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Classes.BussinesLogic
{
    public class FileDTO
    {

        public string Id { get; set; }
        public IFormFile File { get; set; }

    }
}
