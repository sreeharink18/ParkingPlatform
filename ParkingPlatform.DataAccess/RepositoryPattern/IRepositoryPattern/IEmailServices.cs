using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ParkingPlatform.Model.DTO.EmailDtosFolder;

namespace ParkingPlatform.DataAccess.RepositoryPattern.IRepositoryPattern
{
    public interface IEmailServices
    {
        Task SendEmailAsync(Message message, string scenario);
    }
}
