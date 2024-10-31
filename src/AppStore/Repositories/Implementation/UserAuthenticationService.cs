using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AppStore.Models.Domain;
using AppStore.Models.DTO;
using AppStore.Repositories.Abstract;
using Microsoft.AspNetCore.Identity;

namespace AppStore.Repositories.Implementation
{
    public class UserAuthenticationService : IUserAuthenticationService
    {

        private readonly UserManager<AplicationUser>? _userManager;
        private readonly SignInManager<AplicationUser>? _signInManager;

        public UserAuthenticationService(UserManager<AplicationUser> userManager, SignInManager<AplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }



        public async Task<Status> LoginAsync(LoginModel login)
        {
            var status = new Status();
            var user = await _userManager!.FindByNameAsync(login.Username!);

            if (user == null)
            {
                status.StatusCode = 0;
                status.Message = "Error, credenciales inválidas";
                return status;
            }

            if (!await _userManager.CheckPasswordAsync(user, login.Password!))
            {
                status.StatusCode = 0;
                status.Message = "Error, credenciales inválidas";
                return status;
            }

            var result = await _signInManager!.PasswordSignInAsync(user, login.Password!, true, false);

            if (!result.Succeeded)
            {
                status.StatusCode = 0;
                status.Message = "Error, credenciales incorrectas";
                return status;
            }


            status.StatusCode = 1;
            status.Message = "Fue exitoso el login";

            return status;

        }

        public async Task LogoutAsync()
        {
            await _signInManager!.SignOutAsync();
        }
    }
}