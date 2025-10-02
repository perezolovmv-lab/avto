using System;
using System.Collections.Generic;
using System.Linq;
using AutoServiceApp.Domain.Entities;
using AutoServiceApp.Domain.Enums;
using Data.Interfaces;

namespace Data.InMemory.Repositories
{
    public class RepairRequestRepository : IRepairRequestRepository
    {
        private readonly List<RepairRequest> _requests;
        private int _nextId = 1;

        public RepairRequestRepository()
        {
            _requests = new List<RepairRequest>();
            InitializeTestData();
        }

        private void InitializeTestData()
        {
            var testRequest1 = new RepairRequest
            {
                CarBrand = "Toyota",
                CarModel = "Camry",
                ProblemDescription = "Замена масла и фильтров",
                ClientName = "Иванов Иван Иванович",
                PhoneNumber = "+7 (999) 123-45-67",
                Status = RequestStatus.Completed
            };
            Add(testRequest1);

            var testRequest2 = new RepairRequest
            {
                CarBrand = "Honda",
                CarModel = "Civic",
                ProblemDescription = "Диагностика двигателя",
                ClientName = "Петров Петр Петрович",
                PhoneNumber = "+7 (999) 987-65-43",
                Status = RequestStatus.InProgress
            };
            Add(testRequest2);

            var testRequest3 = new RepairRequest
            {
                CarBrand = "Lada",
                CarModel = "Vesta",
                ProblemDescription = "Замена тормозных колодок",
                ClientName = "Сидорова Анна Ивановна",
                PhoneNumber = "+7 (999) 555-35-35",
                Status = RequestStatus.New
            };
            Add(testRequest3);
        }

        public int Add(RepairRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            request.Id = _nextId++;
            request.CreatedDate = DateTime.Now;

            if (request.Status == default(RequestStatus))
                request.Status = RequestStatus.New;

            _requests.Add(request);
            return request.Id;
        }

        public RepairRequest GetById(int id)
        {
            return _requests.FirstOrDefault(r => r.Id == id);
        }

        public List<RepairRequest> GetAll(string searchText = null, RequestStatus? status = null)
        {
            var query = _requests.AsEnumerable();

            if (!string.IsNullOrEmpty(searchText))
            {
                var searchLower = searchText.ToLower();
                query = query.Where(r =>
                    r.Id.ToString().Contains(searchLower) ||
                    r.ClientName.ToLower().Contains(searchLower) ||
                    r.CarBrand.ToLower().Contains(searchLower) ||
                    r.CarModel.ToLower().Contains(searchLower) ||
                    r.ProblemDescription.ToLower().Contains(searchLower));
            }

            if (status.HasValue)
            {
                query = query.Where(r => r.Status == status.Value);
            }

            return query.ToList();
        }

        public bool Update(RepairRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            var existing = GetById(request.Id);
            if (existing == null)
                return false;

            existing.CarBrand = request.CarBrand;
            existing.CarModel = request.CarModel;
            existing.ProblemDescription = request.ProblemDescription;
            existing.ClientName = request.ClientName;
            existing.PhoneNumber = request.PhoneNumber;
            existing.Status = request.Status;

            return true;
        }

        public bool Delete(int id)
        {
            var request = GetById(id);
            return request != null && _requests.Remove(request);
        }
    }
}