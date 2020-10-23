using DotNetNuke.Entities.Modules;
using DotNetNuke.Services.FileSystem;
using DotNetNuke.Services.Log.EventLog;
using System;
using System.Data;

namespace nuce.web.thi
{
    public class CoreModule : PortalModuleBase
    {
        #region Initialization
        #region LoaiCauHoi
        public LoaiCauHoi GetLoaiCauHoi(int Id)
        {
            string cacheKey;
            LoaiCauHoi objLoaiCauHoi=new LoaiCauHoi();
            cacheKey = "nuce_web_thi_loaicauhoi_" + Id.ToString();
            if (Cache[cacheKey] == null)
            {
                DataTable objTable = InitCacheLoaiCauHoi();
                DataRow[] drs = objTable.Select(string.Format("ID = {0}", Id));
                if (drs.Length > 0)
                {
                    objLoaiCauHoi.ID = Id;
                    objLoaiCauHoi.Description = drs[0]["Description"].ToString();
                    objLoaiCauHoi.Name= drs[0]["Name"].ToString();
                }
                else
                {
                    objLoaiCauHoi.ID = Id;
                    objLoaiCauHoi.Description = "";
                    objLoaiCauHoi.Name = "";
                }
                Cache.Insert(cacheKey, objLoaiCauHoi);
            }
            else
            {
                objLoaiCauHoi = (LoaiCauHoi)Cache[cacheKey];
            }
            return objLoaiCauHoi;
        }
        public LoaiCauHoi GetLoaiCauHoi(string Name)
        {
            DataTable objTable = InitCacheLoaiCauHoi();
            DataRow[] drs = objTable.Select(string.Format("Name = '{0}'", Name));
            LoaiCauHoi objLoaiCauHoi = new LoaiCauHoi();
            if (drs.Length > 0)
            {
                objLoaiCauHoi.ID = int.Parse(drs[0]["ID"].ToString());
                objLoaiCauHoi.Description = drs[0]["Description"].ToString();
                objLoaiCauHoi.Name = Name;
            }
            else
            {
                objLoaiCauHoi.ID = -1;
                objLoaiCauHoi.Description = "";
                objLoaiCauHoi.Name = Name;
            }
            return objLoaiCauHoi;
        }
        public DataTable InitCacheLoaiCauHoi()
        {
            string cacheKey;
            cacheKey = "nuce_web_thi_loaicauhoi";
            DataTable dt;
            if (Cache[cacheKey] == null)
            {
                dt = nuce.web.data.dnn_NuceThi_LoaiCauHoi.get(-1);
                Cache.Insert(cacheKey, dt);
            }
            else
                dt=(DataTable)Cache[cacheKey];
            return dt;
        }
        #endregion
        #region DoKho
        public DoKho GetDoKho(string Id)
        {
            return GetDoKho(int.Parse(Id));
        }
        public DoKho GetDoKho(int Id)
        {
            string cacheKey;
            DoKho objDoKho = new DoKho();
            cacheKey = "nuce_web_thi_dokho_" + Id.ToString();
            if (Cache[cacheKey] == null)
            {
                DataTable objTable = InitCacheDoKho();
                DataRow[] drs = objTable.Select(string.Format("DoKhoID={0}", Id));
                if (drs.Length > 0)
                {
                    objDoKho.DoKhoID = Id;
                    objDoKho.Ten = drs[0]["Ten"].ToString();
                }
                else
                {
                    objDoKho.DoKhoID = Id;
                    objDoKho.Ten = "";
                }
                Cache.Insert(cacheKey, objDoKho);
            }
            else
            {
                objDoKho = (DoKho)Cache[cacheKey];
            }
            return objDoKho;
        }
        public DataTable InitCacheDoKho()
        {
            string cacheKey;
            cacheKey = "nuce_web_thi_dokho";
            DataTable dt;
            if (Cache[cacheKey] == null)
            {
                dt = nuce.web.data.dnn_NuceThi_DoKho.get(-1);
                Cache.Insert(cacheKey, dt);
            }
            else
                dt = (DataTable)Cache[cacheKey];
            return dt;
        }
        #endregion
        #endregion

        /// <summary>
        /// Decide if the current user is a super user.
        /// </summary>
        /// <returns>true if the current user is a host user or an admin user.</returns>
        public bool GetSuperMode()
        {
            return this.UserInfo.IsSuperUser;
        }

        /// <summary>
        /// Load int setting
        /// </summary>
        /// <param name="strKey">setting key</param>
        /// <param name="defaultValue">the default value (usually the setting default value)</param>
        /// <returns>the setting int value</returns>
        public int LoadIntSetting(string strKey, int defaultValue)
        {
            int intResult;
            if (Settings[strKey] == null || !int.TryParse(Settings[strKey].ToString(), out intResult))
                intResult = defaultValue;
            return intResult;
        }

        /// <summary>
        /// Load string setting
        /// </summary>
        /// <param name="strKey">setting key</param>
        /// <param name="defaultValue">the default value (usually the setting default value)</param>
        /// <returns>the setting string value</returns>
        public string LoadStringSetting(string strKey, string defaultValue)
        {
            string strResult;

            if (Settings[strKey] == null)
                strResult = defaultValue;
            else strResult = Settings[strKey].ToString();

            return strResult;
        }

        public string LoadXSLSettings(string strKey, string defaultValue, bool blnDefaultXsl)
        {
            string customXsl;
            if (Settings[strKey] == null)
                customXsl = defaultValue;
            else
            {
                string xslPath = string.Format("{0}{1}", PortalSettings.HomeDirectoryMapPath, Settings[strKey].ToString());
                if (Settings[strKey].ToString().Length == 0 || !System.IO.File.Exists(xslPath))
                    customXsl = defaultValue;
                else
                {
                    customXsl = Settings[strKey].ToString();
                    customXsl = "~/Portals/" + this.PortalId.ToString() + "/" + customXsl;
                }
            }
            if (blnDefaultXsl)
                customXsl = defaultValue;

            return customXsl;
        }

        public string LoadXSLReplacement(string defaultValue, string replaceValue)
        {
            string customXsl;

            string xslPath = string.Format("{0}{1}", PortalSettings.HomeDirectoryMapPath, replaceValue);
            if (replaceValue.ToString().Length == 0 || !System.IO.File.Exists(xslPath))
                customXsl = defaultValue;
            else
            {
                string directory = "/Portals/" + this.PortalId.ToString() + "/";
                if (replaceValue.Contains(directory))
                {
                    customXsl = "~" + replaceValue;
                }
                else
                {
                    customXsl = "~/Portals/" + this.PortalId.ToString() + "/" + replaceValue;
                }

            }

            return customXsl;
        }

        /// <summary>
        /// Load bool setting
        /// </summary>
        /// <param name="strSettingKey">the setting key</param>
        /// <returns>if the setting value is found, return it, otherwise return true.</returns>
        public bool LoadBaseTrueSetting(string strSettingKey)
        {
            bool blReturnResult;
            if (Settings[strSettingKey] == null)
                blReturnResult = true;
            else
                blReturnResult = "True".Equals(Settings[strSettingKey]);
            return blReturnResult;
        }

        public DateTime LoadDateTimeSetting(string strSettingKey, string dtmDefault)
        {
            DateTime result = DateTime.Now;
            if (Settings[strSettingKey] == null || !DateTime.TryParse(Settings[strSettingKey].ToString(), out result))
            {
                result = DateTime.Parse(dtmDefault);
            }

            return result;
        }

        /// <summary>
        /// Load bool setting
        /// </summary>
        /// <param name="strSettingKey">the setting key</param>
        /// <returns>if the setting value is found, return it, otherwise return false.</returns>
        public bool LoadBaseFalseSetting(string strSettingKey)
        {
            bool blReturnResult;
            blReturnResult = "True".Equals(Settings[strSettingKey]);
            return blReturnResult;
        }
        public void writeLog(string type, string log)
        {
            EventLogController eventLog = new EventLogController();
            DotNetNuke.Services.Log.EventLog.LogInfo logInfo = new LogInfo();
            logInfo.LogUserID = UserId;

            logInfo.LogPortalID = PortalSettings.PortalId;
            logInfo.LogTypeKey = EventLogController.EventLogType.ADMIN_ALERT.ToString();
            logInfo.AddProperty("tabid=", this.TabId.ToString());
            logInfo.AddProperty("moduleid=", this.ModuleId.ToString());
            logInfo.AddProperty("Loai=", type);
            logInfo.AddProperty("ThongTin=", log);
            eventLog.AddLog(logInfo);
        }


        public  string GetFilePath(string strFile, int portalId)
        {
            if (strFile != "")
            {
                FileController objFileController = new FileController();
                DotNetNuke.Services.FileSystem.FileInfo objXSLFile = objFileController.GetFileById(int.Parse(strFile.Replace("FileID=", "")), portalId);
                return string.Format("{0}{1}", objXSLFile.Folder, objXSLFile.FileName);
            }
            else
            {
                return "";
            }
        }

        public  string GetFileID(string fileUrl, int portalId)
        {
            if (fileUrl != "")
            {
                FileController objFileController = new FileController();
                int intFileID = objFileController.ConvertFilePathToFileId(string.Format("{0}", fileUrl), portalId);
                return string.Format("FileID={0}", intFileID);
            }
            else
            {
                return "";
            }
        }
    }
}