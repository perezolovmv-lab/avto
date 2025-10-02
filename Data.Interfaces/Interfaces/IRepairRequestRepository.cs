using System.Collections.Generic;
using AutoServiceApp.Domain.Entities;
using AutoServiceApp.Domain.Enums;

namespace Data.Interfaces
{
    public interface IRepairRequestRepository
    {
        int Add(RepairRequest request);
        RepairRequest GetById(int id);
        List<RepairRequest> GetAll(string searchText = null, RequestStatus? status = null);
        bool Update(RepairRequest request);
        bool Delete(int id);
    }
}