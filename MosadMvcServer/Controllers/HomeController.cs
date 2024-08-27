using Microsoft.AspNetCore.Mvc;
using MosadMvcServer.DTO;
using MosadMvcServer.Models;
using MosadMvcServer.Services;
using MosadMvcServer.ViewModels;
using System.Diagnostics;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MosadMvcServer.Controllers
{
    public class HomeController : Controller
    {
        private readonly StatisticsService _statisticsService;
        private readonly ActionService _actionService;

        public HomeController( StatisticsService statisticsService , ActionService actionService , HttpJsonService httpJsonService)
        {
            _statisticsService = statisticsService;
            _actionService = actionService;
            TokenService.InitToken(httpJsonService);
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
                Response.Headers.Append("Refresh", "5");

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
                return RedirectToAction("Index", new { errors = ex.Message });
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
                return RedirectToAction("Index", new { errors = "error fetching mission details" +  ex.Message });
            }
        }

        public async Task<IActionResult> TargetsStat()
        {
            try
            {
                List<Target> list = await _statisticsService.GetAllTargets();
                Response.Headers.Append("Refresh", "5");

                return View(list);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return RedirectToAction("Index", new { errors = ex.Message });
            }
        }

        public async Task<IActionResult> MissionControl(string? errors)
        {
            try
            {
                List<Mission> list = await _statisticsService.GetCompatibleMissions();
                ViewBag.Errors = errors;
                Response.Headers.Append("Refresh", "5");

                return View(list);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return RedirectToAction("Index", new { errors = ex.Message });
            }
        }

        public async Task<IActionResult> MatrixView()
        {
            try {
                var agents = await _statisticsService.GetAgentsWithMissionId();
                var targets = await _statisticsService.GetAllTargets();

                var vm = new AgentsWithTargetsVM() { AgentsWithMissionId = agents , targets = targets };
                Response.Headers.Append("Refresh", "5");
                return View(vm);
            }
            catch (HttpRequestException e)
            {
                return RedirectToAction("Index", new
                {
                    errors = e.Message + e.HttpRequestError.ToString()
                });
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return RedirectToAction("Index", new { errors = "error assigning mission: " + ex.Message });
            }
        }

        public async Task<IActionResult> AssignMission(int id, string? errors)
        {
            try
            {
                await _actionService.AssignMission(id);
                ViewBag.Errors = errors;
                return RedirectToAction("MissionControl");
            }
            catch (HttpRequestException e)
            {
                return RedirectToAction("MissionControl", new
                {
                    errors = e.Message + e.HttpRequestError.ToString()
                });
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return RedirectToAction("MissionControl", new { errors = "error assigning mission: " + ex.Message });
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
