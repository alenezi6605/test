using System;
using TestsService.Domain.Common;

namespace TestsService.AppCore.Respositories.Interfaces
{
	public interface IUserRespository
	{
        public Task<EntityResponseModel> GetUsers();
    }
}

