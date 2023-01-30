using FISCA.Authentication;
using FISCA.DSAUtil;
using System.Collections.Generic;
using System.Xml;

namespace ElectronicPaper
{
    public static class EditElectronicPaper
    {
        public static string Insert(string name, string schoolYear, string semester, string viewerType, Dictionary<string, string> metadata)
        {
            var dsreq = new DSXmlHelper("Request");
            dsreq.AddElement("ElectronicPaper");
            dsreq.AddElement("ElectronicPaper", "Name", name);
            dsreq.AddElement("ElectronicPaper", "SchoolYear", schoolYear);
            dsreq.AddElement("ElectronicPaper", "Semester", semester);
            dsreq.AddElement("ElectronicPaper", "ViewerType", viewerType);

            if (metadata != null)
            {
                var hlpmd = new DSXmlHelper("Metadata");
                foreach (KeyValuePair<string, string> each in metadata)
                {
                    XmlElement item = hlpmd.AddElement("Item");
                    item.SetAttribute("Name", each.Key);
                    item.SetAttribute("Value", each.Value);
                }
                dsreq.AddElement("ElectronicPaper", hlpmd.BaseElement);
            }
            
            DSResponse dsrsp = DSAServices.CallService("SmartSchool.ElectronicPaper.Insert", new DSRequest(dsreq));
            if (dsrsp.HasContent)
            {
                DSXmlHelper helper = dsrsp.GetContent();
                string newid = helper.GetText("NewID");
                return newid;
            }
            return "";
        }

        public static void UpdatePaperName(string new_name, string id)
        {
            var dsreq = new DSXmlHelper("Request");
            dsreq.AddElement("ElectronicPaper");
            dsreq.AddElement("ElectronicPaper", "Name", new_name);
            dsreq.AddElement("ElectronicPaper", "Condition");
            dsreq.AddElement("ElectronicPaper/Condition", "ID", id);
            DSAServices.CallService("SmartSchool.ElectronicPaper.Update", new DSRequest(dsreq));
        }

        public static void Delete(params string[] ids)
        {
            var dsreq = new DSXmlHelper("Request");
            dsreq.AddElement("ElectronicPaper");
            foreach (var id in ids)
                dsreq.AddElement("ElectronicPaper", "ID", id);
            DSAServices.CallService("SmartSchool.ElectronicPaper.Delete", new DSRequest(dsreq));
        }

        public static void InsertPaperItem(DSXmlHelper request)
        {
            DSAServices.CallService("SmartSchool.ElectronicPaper.InsertPaperItem", new DSRequest(request));
        }

        public static void DeletePaperItem(params string[] item_ids)
        {
            var helper = new DSXmlHelper("Request");
            helper.AddElement("Paper");
            foreach (var each_id in item_ids)
                helper.AddElement("Paper", "PaperItemID", each_id);
            DSAServices.CallService("SmartSchool.ElectronicPaper.DeletePaperItem", new DSRequest(helper));
        }
    }
}