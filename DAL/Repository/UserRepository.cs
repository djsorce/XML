using DAL.Helpers;
using DAL.Interfaces;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace DAL.Repository
{

    public class UserRepository : IUserRepository
    {

        public async Task<bool?> AddUserToXML(UserDetail User)
        {
            bool isCompleted = false;
            try
            {
                //If the file doesn't exist create a new file Else append existing file
                if (!File.Exists(ConstantStrings.FilePath))
                {
                    XmlWriterSettings xmlWriterSettings = new XmlWriterSettings();
                    xmlWriterSettings.Indent = true;
                    xmlWriterSettings.NewLineOnAttributes = true;
                    using (XmlWriter xmlWriter = XmlWriter.Create(ConstantStrings.FilePath, xmlWriterSettings))
                    {
                        xmlWriter.WriteStartDocument();
                        xmlWriter.WriteStartElement(ConstantStrings.Users);
                        xmlWriter.WriteStartElement(ConstantStrings.User);
                        xmlWriter.WriteElementString(ConstantStrings.Name, User.name);
                        xmlWriter.WriteElementString(ConstantStrings.Surname, User.surname);
                        xmlWriter.WriteElementString(ConstantStrings.CellphoneNumber, User.cellphone);
                        xmlWriter.WriteEndElement();

                        xmlWriter.WriteEndElement();
                        xmlWriter.WriteEndDocument();
                        xmlWriter.Flush();
                        xmlWriter.Close();
                    }
                }
                else
                {
                    XDocument xDocument = await Task.Run(() => XDocument.Load(ConstantStrings.FilePath));
                    XElement root = xDocument.Element(ConstantStrings.Users)!;

                    IEnumerable<XElement> rows = root.Descendants(ConstantStrings.User);
                    XElement firstRow = rows.First();
                    firstRow.AddBeforeSelf(
                       new XElement(ConstantStrings.User,
                       new XElement(ConstantStrings.Name, User.name),
                       new XElement(ConstantStrings.Surname, User.surname),
                       new XElement(ConstantStrings.CellphoneNumber, User.cellphone)));
                    xDocument.Save(ConstantStrings.FilePath);
                }
                isCompleted = true;
            }
            catch (Exception)
            {
                isCompleted = false;
            }
            return isCompleted;
        }


        public async Task<List<UserDetail>?> GetListOfUsers()
        {
            List<UserDetail>? Response = null;
            UserDetail? userDetail = null;
            try
            {
                //Check if the file exists and return a list of users
                if (File.Exists(ConstantStrings.FilePath))
                {
                    XDocument xDocument = await Task.Run(() => XDocument.Load(ConstantStrings.FilePath));
                    XElement root = xDocument.Element(ConstantStrings.Users)!;
                    IEnumerable<XElement> rows = root.Descendants(ConstantStrings.User);
                    Response = new List<UserDetail>();
                    foreach (XElement row in rows)
                    {
                        userDetail = new UserDetail
                        {
                            name = row.Descendants(ConstantStrings.Name).First().Value,
                            surname = row.Descendants(ConstantStrings.Surname).First().Value,
                            cellphone = row.Descendants(ConstantStrings.CellphoneNumber).First().Value,
                        };
                        Response.Add(userDetail);
                    }
                }
            }
            catch (Exception)
            {
                //log exception ex.Message
            }
            return Response;
        }

        public async Task<UserDetail?> GetSingleUserByCell(string Cellphone)
        {
            UserDetail? userDetail = null;
            try
            {
                //Check if the file exists and return a user by cellphone
                if (File.Exists(ConstantStrings.FilePath))
                {
                    XDocument xDocument = await Task.Run(() => XDocument.Load(ConstantStrings.FilePath));
                    XElement root = xDocument.Element(ConstantStrings.Users)!;
                    IEnumerable<XElement> rows = root.Descendants(ConstantStrings.User);
                    foreach (XElement row in rows)
                    {
                        if (row.Descendants(ConstantStrings.CellphoneNumber).First().Value == Cellphone)
                        {
                            userDetail = new UserDetail
                            {
                                name = row.Descendants(ConstantStrings.Name).First().Value,
                                surname = row.Descendants(ConstantStrings.Surname).First().Value,
                                cellphone = row.Descendants(ConstantStrings.CellphoneNumber).First().Value,
                            };
                        }
                        else
                        {
                            //do nothing here
                        }
                    }
                }
            }
            catch (Exception)
            {
                //log exception ex.Message
            }
            return userDetail;
        }


        public async Task<bool?> DeleteUserByCell(string Cellphone)
        {
            bool isCompleted = false;
            try
            {
                //Check if the file exists and return a list of users
                if (File.Exists(ConstantStrings.FilePath))
                {
                    XDocument xDocument = await Task.Run(() => XDocument.Load(ConstantStrings.FilePath));
                    XElement root = xDocument.Element(ConstantStrings.Users)!;
                    IEnumerable<XElement> rows = root.Descendants(ConstantStrings.User);
                    foreach (XElement row in rows)
                    {
                        if (row.Descendants(ConstantStrings.CellphoneNumber).First().Value == Cellphone)
                        {
                            //Check if the file will have anyrows left after deleting the last row otherwise dont allow the delete to complete
                            if (rows.Count() > 1)
                            {
                                row.Remove();
                                isCompleted = true;
                                break;
                            }
                            else
                            {
                                isCompleted = false;
                                break;
                            }
                        }
                    }
                    xDocument.Save(ConstantStrings.FilePath);
                }
            }
            catch (Exception)
            {
                //log exception ex.Message
            }
            return isCompleted;
        }


        public async Task<bool> UpdateUserDetail(UserDetail User, string PreviousCellphone)
        {
            bool isUpdated = false;
            try
            {
                if (File.Exists(ConstantStrings.FilePath))
                {
                    XDocument xDocument = await Task.Run(() => XDocument.Load(ConstantStrings.FilePath));
                    XElement root = xDocument.Element(ConstantStrings.Users)!;
                    IEnumerable<XElement> rows = root.Descendants(ConstantStrings.User);
                    foreach (XElement row in rows)
                    {
                        if (row.Descendants(ConstantStrings.CellphoneNumber).First().Value == PreviousCellphone)
                        {
                            row.Descendants(ConstantStrings.CellphoneNumber).First().Value = User.cellphone!;
                            row.Descendants(ConstantStrings.Name).First().Value = User.name!;
                            row.Descendants(ConstantStrings.Surname).First().Value = User.surname!;
                        }
                        else
                        {
                            //do nothing here
                        }
                    }
                    xDocument.Save(ConstantStrings.FilePath);
                }
                isUpdated = true;
            }
            catch (Exception)
            {
                //log exception ex.Message
            }
            return isUpdated;
        }
    }

}
