using DailySpin.DataProvider;
using DailySpin.DataProvider.Data;
using DailySpin.DataProvider.Enums;
using DailySpin.DataProvider.Helpers;
using DailySpin.DataProvider.Response;
using DailySpin.Logic.Interfaces;
using DailySpin.ViewModel.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace DailySpin.Logic.Services
{
    public class AccountService : IAccountService
    {
        public IUnitOfWork _unitOfWork;
        private readonly ILogger<AccountService> _logger;

        public AccountService(IUnitOfWork unitOfWork,
            ILogger<AccountService> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<BaseResponse<ClaimsIdentity>> Register(RegisterViewModel model)
        {
            try
            {
                var user = await _unitOfWork.UserRepository.GetAll().FirstOrDefaultAsync(x => x.Email == model.Email);
                var userName = await _unitOfWork.UserRepository.GetAll().FirstOrDefaultAsync(x => x.DisplayName == model.Nickname);
                if (user != null)
                {
                    return new BaseResponse<ClaimsIdentity>()
                    {
                        Description = "User with this login already exist"
                    };
                }

                if (userName != null)
                {
                    return new BaseResponse<ClaimsIdentity>()
                    {
                        Description = "User with this nickname already exist"
                    };
                }

                MemoryStream target = new MemoryStream();
                model.Image.CopyTo(target);
                byte[] data = target.ToArray();
                user = new UserAccount()
                {
                    Email = model.Email,
                    DisplayName = model.Nickname,
                    Role = Role.User,
                    Password = HashPasswordHelper.HashPassword(model.Password),
                    Balance = 0,
                    Image = data
                };
                _unitOfWork.UserRepository.Create(user);
                var result = Authenticate(user);
                _unitOfWork.Commit();
                return new BaseResponse<ClaimsIdentity>()
                {
                    Data = result,
                    Description = "Object added",
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception)
            {
                return new BaseResponse<ClaimsIdentity>()
                {
                    Description = "[Register]: FATAL ERROR",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public async Task<BaseResponse<ClaimsIdentity>> Login(LoginViewModel model)
        {
            try
            {
                var user = await _unitOfWork.UserRepository.GetAll().FirstOrDefaultAsync(x => x.Email == model.Email);
                if (user == null)
                {
                    return new BaseResponse<ClaimsIdentity>()
                    {
                        Description = "User not found"
                    };
                }

                if (user.Password != HashPasswordHelper.HashPassword(model.Password))
                {
                    return new BaseResponse<ClaimsIdentity>()
                    {
                        Description = "Error: check login or pass"
                    };
                }
                var result = Authenticate(user);

                return new BaseResponse<ClaimsIdentity>()
                {
                    Data = result,
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception)
            {
                return new BaseResponse<ClaimsIdentity>()
                {
                    Description = "[Login]: FATAL ERROR",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public async Task<BaseResponse<bool>> ChangePassword(ChangePasswordViewModel model)
        {
            try
            {
                var user = await _unitOfWork.UserRepository.GetAll().FirstAsync(x => x.Email == model.Email);
                if (user == null)
                {
                    return new BaseResponse<bool>()
                    {
                        StatusCode = StatusCode.UserNotFound,
                        Description = "User not found"
                    };
                }

                user.Password = HashPasswordHelper.HashPassword(model.NewPassword);
                _unitOfWork.UserRepository.Update(user);
                _unitOfWork.Commit();
                return new BaseResponse<bool>()
                {
                    Data = true,
                    StatusCode = StatusCode.OK,
                    Description = "Password has been changed"
                };

            }
            catch (Exception)
            {
                return new BaseResponse<bool>()
                {
                    Description = "[ChangePassword]: FATAL ERROR",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        private ClaimsIdentity Authenticate(UserAccount user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.DisplayName),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, user.Role.ToString()),
            };

            return new ClaimsIdentity(claims, "ApplicationCookie",
                ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
        }

        public async Task<BaseResponse<BetViewModel>> LoadUserData(string loginedUser)
        {
            try
            {
                var user = await _unitOfWork.UserRepository.GetAll().FirstAsync(x => x.DisplayName == loginedUser);
                if (user == null)
                {
                    return new BaseResponse<BetViewModel>()
                    {
                        Description = "No any users",
                        StatusCode = StatusCode.UserNotFound
                    };
                }
                BetViewModel returnedUser = new BetViewModel();

                returnedUser.UserImage = user.Image!;
                returnedUser.UserName = user.DisplayName;
                returnedUser.UserBalance = user.Balance;

                return new BaseResponse<BetViewModel>()
                {
                    Data = returnedUser
                };

            }
            catch (Exception)
            {
                return new BaseResponse<BetViewModel>()
                {
                    Description = "[LoadUserData]: FATAL ERROR",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public async Task<BaseResponse<bool>> Deposit(string loginedUser, ulong sum)
        {
            try
            {
                var user = await _unitOfWork.UserRepository.GetAll().FirstAsync(x => x.DisplayName == loginedUser);
                if (user == null || sum < 1)
                {
                    return new BaseResponse<bool>()
                    {
                        Data = false,
                        Description = "No any users or sum < 1",
                        StatusCode = StatusCode.InternalServerError
                    };
                }
                user.Balance += sum;
                _unitOfWork.UserRepository.Update(user);
                _unitOfWork.Commit();
                return new BaseResponse<bool>()
                {
                    Data = true,
                    Description = "Successfully deposit",
                    StatusCode = StatusCode.OK
                };

            }
            catch (Exception)
            {
                return new BaseResponse<bool>()
                {
                    Data = false,
                    Description = "[Deposit]: FATAL ERROR",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public async Task<BaseResponse<bool>> Withdraw(string loginedUser, ulong sum)
        {
            try
            {
                var user = await _unitOfWork.UserRepository.GetAll().FirstAsync(x => x.DisplayName == loginedUser);
                if (user == null || sum < 1)
                {
                    return new BaseResponse<bool>()
                    {
                        Data = false,
                        Description = "No any users or sum < 1",
                        StatusCode = StatusCode.InternalServerError
                    };
                }
                if (sum <= user.Balance)
                {
                    user.Balance -= sum;
                    _unitOfWork.UserRepository.Update(user);
                }
                _unitOfWork.Commit();
                return new BaseResponse<bool>()
                {
                    Data = true,
                    Description = "Successfully withdraw",
                    StatusCode = StatusCode.OK
                };

            }
            catch (Exception)
            {
                return new BaseResponse<bool>()
                {
                    Data = false,
                    Description = "[Withdraw]: FATAL ERROR",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }
    }
}