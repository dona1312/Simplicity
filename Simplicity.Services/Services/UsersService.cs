﻿using AutoMapper;
using Simplicity.DataContracts.Dtos;
using Simplicity.DataContracts.Dtos.Users;
using Simplicity.Entities;
using Simplicity.Repositories.Repositories;
using Simplicity.Repositories.RepositoryInterfaces;
using Simplicity.Services.ServicesInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Simplicity.Services.Services
{
    public class UsersService : BaseService<User>, IUsersService
    {
        private readonly IUsersRepository _usersRepository;
        private readonly ITicketsService _ticketsService;
        private readonly IMapper _mapper;

        public UsersService(IUsersRepository usersRepository,
            ITicketsService ticketsService,
            IMapper mapper) : base(usersRepository)
        {
            _usersRepository = usersRepository;
            _ticketsService = ticketsService;
            _mapper = mapper;
        }

        List<UserListDto> IUsersService.GetAllUserDtos(Expression<Func<User, bool>> filter)
        {
            IBaseRepository<User> repo = new UsersRepository();

            return _usersRepository.GetAllUserDtos(filter);
        }

        List<NameAndIDDto> IUsersService.GetAllUserNameAndIdDtos(Expression<Func<User, bool>> filter)
        {
            return _usersRepository.GetAllUserNameAndIdDtos(filter);
        }
        
        public void HashUserPassword(UserEditDto user)
        {
            // update password if it was entered
            if (!string.IsNullOrWhiteSpace(user.Password))
            {
                byte[] passwordHash, passwordSalt;
                CreatePasswordHash(user.Password, out passwordHash, out passwordSalt);

                user.PasswordHash = passwordHash;
                user.PasswordSalt = passwordSalt;
            }
        }

        private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            if (password == null) throw new ArgumentNullException("password");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");

            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private void CovarianceExample()
        {
            IUserActivityService<BaseEntitity> userActivityService = new UserActivityService();
            var userActivity = userActivityService.GetUserActivity();
        }

        private void ContravarianceExample()
        {
            IUserNotificationService<User> service = new UserNotificationService();
            service.SetData(new User());
        }

        public bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
        {
            if (password == null) throw new ArgumentNullException("password");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");
            if (storedHash.Length != 64) throw new ArgumentException("Invalid length of password hash (64 bytes expected).", "passwordHash");
            if (storedSalt.Length != 128) throw new ArgumentException("Invalid length of password salt (128 bytes expected).", "passwordHash");

            using (var hmac = new System.Security.Cryptography.HMACSHA512(storedSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != storedHash[i]) return false;
                }
            }

            return true;
        }
        
        public void SaveUser(UserEditDto userEditDto)
        {
            var entity = new User();

            if (userEditDto.ID != 0)
            {
                entity = this.GetById(userEditDto.ID);
            }

            entity.Name = userEditDto.Name;
            entity.Address = userEditDto.Address;
            entity.Role = userEditDto.Role;

            _mapper.Map(userEditDto, entity);
            this.Save(entity);
        }

        public UserEditDto GetUserEditDtoById(int userId)
        {
            var user = this.GetById(userId);
            return _mapper.Map(user, new UserEditDto());
        }

        public UserListDto GetUserListDtoById(int userId)
        {
            var user = this.GetById(userId);
            return _mapper.Map(user, new UserListDto());
        }

        public string Validate(int userId)
        {
            if (this.GetById(userId) == null)
            {
                return "User does not exists";
            }

            if (_ticketsService.GetAllTaskDtos(x=> x.CreatorID == userId || x.AssigneeID == userId)
                .Any())
            {
                return "User cannot be deleted. User is has active tickets to resolve.";
            }

            return string.Empty;
        }
    }
}
