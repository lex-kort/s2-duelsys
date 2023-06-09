using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;

using DAL;
using DAL.Repositories;
using BLL.Objects;
using BLL.Registries; 
using BLL.Enums;

namespace WebApp.Pages.Tournaments
{
    public class ListModel : PageModel
    {
        private TournamentRegistry registry = new TournamentRegistry(new TournamentRepository(new DbContext()));

        [BindProperty]
        public List<Tournament>? Tournaments { get; set; }

        public void OnGet()
        {
            Tournaments = registry.GetAll(false).ToList();
        }

        public IEnumerable<Tournament> GetByStatus(TournamentStatus status)
        {
            return Tournaments!.Where(t => t.Status == status).OrderBy(t => t.StartDate);
        }
    }
}
