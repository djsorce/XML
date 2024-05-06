using Microsoft.AspNetCore.Mvc;
using DAL.Interfaces;
using DAL.Repository;
using Microsoft.VisualBasic;
using UserCaptureXML.Helpers;
using DAL.Models;
using System.Xml.Linq;

namespace UserCaptureXML.Controllers
{
    public class UpdateController : Controller
    {
        private readonly IUserRepository _usersRepository = new UserRepository();

        [HttpGet]
        public async Task<IActionResult> ExistingUser()
        {
            List<UserDetail>? users = null;
            try
            {
                //Get the information from the XML File using the Data Access Method
                users = await _usersRepository.GetListOfUsers();
            }
            catch (Exception Ex)
            {
                //Do something with the exception here
                //Log exception to error log file or database
                ViewData[ConstantStrings.MsgOK] = Ex.Message.ToString();
            }
            return View(ConstantStrings.ExistingUser, users);
        }


        [HttpGet]
        public async Task<IActionResult> SingleUser(string Key,int ActionKey)
        {
            UserDetail? user = null;
            try
            {
                if (ActionKey == (int)EnumActions.Edit)
                {
                    //Get the information from the XML File using the Data Access Method
                    user = await _usersRepository.GetSingleUserByCell(Key);
                }
                else if (ActionKey == (int)EnumActions.Delete)
                {
                    //if not edit action then delete user
                    var Response = await _usersRepository.DeleteUserByCell(Key);
                    if (Response == true)
                    {
                        TempData[ConstantStrings.MsgOK] = ConstantStrings.SuccessMsgDel;
                    }
                    else
                    {
                        TempData[ConstantStrings.MsgOK] = ConstantStrings.NoSuccessMsg;
                    }
                    return RedirectToAction(ConstantStrings.ExistingUser, ConstantStrings.Update);
                }

            }
            catch (Exception Ex)
            {
                //Do something with the exception here
                //Log exception to error log file or database
                ViewData[ConstantStrings.MsgOK] = Ex.Message.ToString();
            }
            return View(ConstantStrings.SingleUser, user);
        }

        [HttpPost]
        public async Task<IActionResult> SingleUser(UserDetail User, string PreviousCellphone)
        {
            try
            {
                var Response = await _usersRepository.UpdateUserDetail(User, PreviousCellphone);
                if (Response)
                {
                    TempData[ConstantStrings.MsgOK] = ConstantStrings.SuccessMsgUp;
                }
                else
                {
                    TempData[ConstantStrings.MsgOK] = ConstantStrings.NoSuccessMsg;
                }
            }
            catch (Exception Ex)
            {
                //Do something with the exception here
                //Log exception to error log file or database
                ViewData[ConstantStrings.MsgOK] = Ex.Message.ToString();
            }
            return RedirectToAction(ConstantStrings.ExistingUser, ConstantStrings.Update);
        }
    }
}
