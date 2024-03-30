using Mid7_Project_01.Models;
using Mid7_Project_01.Repositories;
using Mid7_Project_01.Repositories.Interfaces;
using Mid7_Project_01.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using X.PagedList;

namespace Mid7_Project_01.Controllers
{
    [Authorize]
    public class TeamsController : Controller
    {
        private readonly TeamDbContext db = new TeamDbContext();
        IGenericRepo<Team> repo;
        public TeamsController()
        {
            this.repo = new GenericRepo<Team>(db);
        }
        // GET: Teams
        public ActionResult Index(int pg=1)
        {
            var data = this.repo.GetAll("TeamMembers").OrderBy(x => x.TeamId).ToPagedList(pg, 5);
            return View(data);
        }
        public ActionResult Create()
        {  TeamInputModel model = new TeamInputModel();
            model.TeamMembers.Add(new TeamMember());
            return View(model);
        }
        [HttpPost]
        public ActionResult Create(TeamInputModel t, string act="")
        {
            if (act == "add")
            {
                t.TeamMembers.Add(new TeamMember() { BirthDate=DateTime.Today});
                foreach (var v in ModelState.Values)
                {
                    v.Errors.Clear();
                }
            }
            if (act.StartsWith("remove"))
            {
                var index = int.Parse(act.Substring(act.IndexOf("_") + 1));
                t.TeamMembers.RemoveAt(index);

                foreach (var v in ModelState.Values)
                {
                    v.Errors.Clear();
                }
            }
            if (act == "insert")
            {
                if (ModelState.IsValid)
                {
                    var team = new Team
                    {
                        TeamName = t.TeamName,
                        Grad = t.Grad,
                        Coach = t.Coach,
                        IsActive = t.IsActive

                    };
                    string ext = Path.GetExtension(t.Picture.FileName);
                    string fileName = Path.GetFileNameWithoutExtension(Path.GetRandomFileName()) + ext;
                    string savePath = Path.Combine(Server.MapPath("~/Pictures"), fileName);
                    t.Picture.SaveAs(savePath);
                    team.Picture = fileName;
                    foreach (var l in t.TeamMembers)
                    {
                        team.TeamMembers.Add(l);
                    }

                    this.repo.Insert(team);
                    t = new TeamInputModel();
                    t.TeamMembers.Add(new TeamMember());
                }
            }
            ViewData["Act"] = act;
            
           
            return PartialView("_CreatePartial", t);
        }
        
        
        [HttpPost]
        public ActionResult Delete(int id)
        {
            var p = this.repo.Get(x => x.TeamId == id, "TeamMembers");//db.Teams.FirstOrDefault(x => x.TeamId == id);
            if (p == null) return new HttpNotFoundResult();
            this.repo.ExecuteCommand($"EXECUTE dbo.DeleteTeam {id}");
            return Json(new { success = true, id });
        }
        public ActionResult Edit(int id)
        {

            var d = this.repo.Get(x => x.TeamId == id); //db.Teams.FirstOrDefault(x => x.TeamId == id);
            if (d == null) return new HttpNotFoundResult();
            var model = new TeamEditModel
            {
                TeamId = d.TeamId,
                
                Grad = d.Grad,
                Coach = d.Coach,
                TeamName = d.TeamName,
                IsActive = d.IsActive


            };
            model.TeamMembers = d.TeamMembers.ToList();
            ViewData["CurrentPic"] = d.Picture;
            

            return View(model);
        }
        [HttpPost]
        public ActionResult Edit(TeamEditModel model, string act = "")
        {
            var team = db.Teams.FirstOrDefault(x => x.TeamId == model.TeamId);
            if (team == null) return new HttpNotFoundResult();
            if (act == "add")
            {
                model.TeamMembers.Add(new TeamMember() { BirthDate=DateTime.Today});
                foreach (var v in ModelState.Values)
                {
                    v.Errors.Clear();
                }
            }
            if (act.StartsWith("remove"))
            {
                var i = int.Parse(act.Substring(act.IndexOf("_") + 1));
                var tt = model.TeamMembers.Find(x=> x.TeamMemberId == i);
                model.TeamMembers.Remove(tt);

                foreach (var v in ModelState.Values)
                {
                    v.Errors.Clear();
                }
            }
            if (act == "update")
            {
                if (ModelState.IsValid)
                {
                    team.TeamId = model.TeamId;
                    team.TeamName = model.TeamName;
                    team.Grad = model.Grad;
                    team.Coach = model.Coach;
                    
                    team.IsActive = model.IsActive;
                    if (model.Picture != null)
                    {
                        string ext = Path.GetExtension(model.Picture.FileName);
                        string fileName = Path.GetFileNameWithoutExtension(Path.GetRandomFileName()) + ext;
                        string savePath = Path.Combine(Server.MapPath("~/Pictures"), fileName);
                        model.Picture.SaveAs(savePath);
                        team.Picture = fileName;
                    }
                    var olds = db.TeamMembers.Where(x => x.TeamId == model.TeamId).ToList();

                    for (var i = 0; i < olds.Count; i++)
                    {
                        db.TeamMembers.Remove(olds[i]);
                        team.TeamMembers.Remove(olds[i]);
                    }

                    foreach (var sp in model.TeamMembers)
                    {
                        team.TeamMembers.Add(new TeamMember { TeamMemberName=sp.TeamMemberName, BirthDate=sp.BirthDate, Phone=sp.Phone});
                    }





                    db.SaveChanges();


                }
            }
            ViewData["Act"] = act;

            ViewData["CurrentPic"] = team.Picture;
           

            return PartialView("_EditPartial", model);
        }

    }
}