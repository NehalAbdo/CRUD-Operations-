using AutoMapper;
using Demo.Dal.Entities;
using Demo.PL.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;


namespace Demo.PL.Controllers
{
	public class UsersController : Controller
	{
		private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;

        public UsersController(UserManager<AppUser> userManager, IMapper mapper)
        {
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task <IActionResult> userIndex(string email)
		{
			if (string.IsNullOrWhiteSpace(email)) 
			{
				var users = await _userManager.Users.Select( u => new UserVMcs
				{
					Id = u.Id,
					Email = u.Email,
					FName = u.FName,
					LName = u.LName,
					Roles = _userManager.GetRolesAsync(u).Result

				}).ToListAsync();
				return View(users);
			}
			var user= await _userManager.FindByEmailAsync(email);
			if (user is null) return View(Enumerable.Empty<UserVMcs>());
			var mappedUser = new UserVMcs
			{
				Email = user.Email,
				FName = user.FName,
				LName = user.LName,
				Roles = await _userManager.GetRolesAsync(user)
			};
			return View(new List<UserVMcs> { mappedUser });
		}
		public async Task<IActionResult> Details(string id, string viewname="Details")
		{
			if (string.IsNullOrWhiteSpace(id)) return BadRequest();
			var user=await _userManager.FindByIdAsync(id);
			if (user is null) return NotFound();
            var mappedUser= _mapper.Map<AppUser, UserVMcs>(user);
			mappedUser.Roles = await _userManager.GetRolesAsync(user);
			return View(viewname,mappedUser);

        }

		public async Task<IActionResult> Edit(string id)
		{
			return await Details(id,nameof(Edit));
		}
		[HttpPost]
        public async Task<IActionResult> Edit(string id , UserVMcs model)
        {
			if(id != model.Id) return BadRequest();
			if(ModelState.IsValid) return View(model);
			try 
			{
				var user = await _userManager.FindByIdAsync(id);
				if (user is null) return NotFound();
				user.FName = model.FName;
				user.LName = model.LName;
				user.Email = model.Email;

				await _userManager.UpdateAsync(user);
				return RedirectToAction("userIndex");
			}
			catch(Exception ex)
			{
				ModelState.AddModelError("", ex.Message);

			}
			return View(model);
        }

        public async Task<IActionResult> Delete(string id)
        {
            return await Details(id, nameof(Delete));
        }

        [HttpPost]
        public async Task<IActionResult> ConfirmDelete(string id)
        {
            
            try
            {
                var user = await _userManager.FindByIdAsync(id);
                if (user is null) return NotFound();
                
                await _userManager.DeleteAsync(user);
                return RedirectToAction("userIndex");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);

            }
            return View();
        }
    }
}
