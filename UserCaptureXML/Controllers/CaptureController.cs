using Microsoft.AspNetCore.Mvc;
using DAL.Interfaces;
using DAL.Repository;
using Microsoft.VisualBasic;
using UserCaptureXML.Helpers;
using DAL.Models;

namespace UserCaptureXML.Controllers
{
    public class CaptureController : Controller
    {
        private readonly IUserRepository _usersRepository = new UserRepository();
        private readonly IConfiguration _config;
        public CaptureController(IConfiguration config)
        {
            _config = config;
        }


        [HttpGet]
        public IActionResult NewUser()
        {
            var RequireUppercase = _config.GetValue<string>("Words:One");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> NewUser(UserDetail User)
        {
            try
            {
                //Add the information to an XML File using the Data Access Method
                var Response = await _usersRepository.AddUserToXML(User);
                if (Response != null)
                {
                    ViewData["SuccessMsg"] = ConstantStrings.SuccessMsg;
                }
                else
                {
                    ViewData["SuccessMsg"] = ConstantStrings.NoSuccessMsg;
                }
            }
            catch (Exception Ex)
            {
                //Do something with the exception here
                ViewData["SuccessMsg"] = Ex.Message.ToString();
            }

            return View();
        }

    }
}
