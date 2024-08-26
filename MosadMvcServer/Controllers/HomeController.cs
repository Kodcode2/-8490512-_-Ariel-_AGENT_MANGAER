using Microsoft.AspNetCore.Mvc;
using MosadMvcServer.DTO;
using MosadMvcServer.Models;
using MosadMvcServer.Services;
using MosadMvcServer.ViewModels;
using System.Diagnostics;

namespace MosadMvcServer.Controllers
{
    public class HomeController : Controller
    {
        private readonly StatisticsService _statisticsService;

        public HomeController( StatisticsService statisticsService)
        {
            _statisticsService = statisticsService;
        }

        public IActionResult Index(string? errors)
        {
            ViewBag.Errors = errors;
            return View();
        }

        public async Task<IActionResult> OverView()
        {
            try
            {
                var viewModel = new StatOverViewModel();
                // get all agents count/ how many active
                var agentsCount = await _statisticsService.GetAgentsCount();
                viewModel.TotalAgents = agentsCount.Item1;
                viewModel.ActiveAgents = agentsCount.Item2;

                //targets/ how many teminated
                var TargetsCount = await _statisticsService.GetTargetCount();
                viewModel.totalTargets = TargetsCount.Item1;
                viewModel.activeTargets = TargetsCount.Item2;

                //missions / how many assined
                var missionsCount = await _statisticsService.GetMissionCount();
                viewModel.totalMissions = missionsCount.Item1;
                viewModel.activeMissions = missionsCount.Item2;

                // agents.count / targets.count
                var ac = viewModel.TotalAgents;
                var tc = viewModel.totalTargets;

                // agents.idle and compatable / targets.! assined
                var CompatableMissionsInfo = await _statisticsService.GetCompatiblePairsCount();
                viewModel.idleAgentCount = CompatableMissionsInfo.Item1;
                viewModel.unassignedTargetCount = CompatableMissionsInfo.Item2;

                return View(viewModel);
            }
            catch (HttpRequestException ex)
            {
                Debug.WriteLine(ex.Message);
                return RedirectToAction("Index", new { errors = "error fetching data from api" });
            }
            catch (Exception ex) 
            {
                Debug.WriteLine(ex);
                return RedirectToAction("Index",new {errors = "error fetching data from api" });
            }

        }

        public async Task<IActionResult> AgentsStat()
        {
            try
            {
                List<AgentWithMissionIdDTO> list = await _statisticsService.GetAgentsWithMissionId();
                return View(list);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return RedirectToAction("Index", new { errors = "error" });
            }
        }

        public async Task<IActionResult> MissionDetails(int id)
        {
            try
            {
                Mission mission = await _statisticsService.GetMission(id);
                return View(mission);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return RedirectToAction("Index", new { errors = "error fetching mission details" });
            }
        }

        public async Task<IActionResult> TargetsStat()
        {
            try
            {
                List<Target> list = await _statisticsService.GetAllTargets();
                return View(list);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return RedirectToAction("Index", new { errors = "error" });
            }
        }

        public async Task<IActionResult> MissionControl()
        {
            try
            {
                List<Agent> list = await _statisticsService.GetAgentsWithMissions();
                return View(list);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return RedirectToAction("Index", new { errors = "error" });
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
