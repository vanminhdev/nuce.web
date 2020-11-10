using EduWebService;
using Microsoft.EntityFrameworkCore;
using nuce.web.api.HandleException;
using nuce.web.api.Models.EduData;
using nuce.web.api.Models.Survey;
using nuce.web.api.Services.Synchronization.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using static EduWebService.ServiceSoapClient;

namespace nuce.web.api.Services.Synchronization.Implements
{
    public class SyncEduDatabaseService : ISyncEduDatabaseService
    {
        private readonly EduDataContext _eduDataContext;
        private readonly ServiceSoapClient srvc = new ServiceSoapClient(EndpointConfiguration.ServiceSoap12);

        public SyncEduDatabaseService(EduDataContext eduDataContext)
        {
            _eduDataContext = eduDataContext;
        }

        public async Task SyncFaculty()
        {
            var transaction = _eduDataContext.Database.BeginTransaction();
            try
            {
                _eduDataContext.Database.ExecuteSqlRaw("TRUNCATE TABLE AS_Academy_Faculty");
                var result = await srvc.getKhoaAsync();
                XmlNodeList listData = result.Any1.GetElementsByTagName("dataKhoa");
                foreach (XmlElement item in listData)
                {
                    var orderString = item.Attributes.GetNamedItem("msdata:rowOrder").InnerText;
                    int order = 1;
                    int.TryParse(orderString, out order);

                    var maKH = item.ChildNodes[0].InnerText;
                    var tenKhoa = item.ChildNodes[1].InnerText;

                    _eduDataContext.AsAcademyFaculty.Add(new AsAcademyFaculty
                    {
                        Code = maKH,
                        Name = tenKhoa,
                        SemesterId = 1,
                        COrder = order + 1
                    });
                }
                await _eduDataContext.SaveChangesAsync();
                transaction.Commit();
            }
            catch (DbUpdateException e)
            {
                throw e;
            }
            catch (Exception e)
            {
                await transaction.RollbackAsync();
                var message = UtilsException.GetMainMessage(e);
                throw new CannotCallEduWebServiceException(message);
            }
        }

        public async Task SyncSubjects()
        {
            try
            {
                var result = await srvc.getMonHocAsync();
            }
            catch
            {

            }
            return;
        }
    }
}
