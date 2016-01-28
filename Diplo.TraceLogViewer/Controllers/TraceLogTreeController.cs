﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Formatting;
using System.Web;
using System.Web.Http;
using Diplo.TraceLogViewer.Services;
using umbraco.BusinessLogic.Actions;
using Umbraco.Core;
using Umbraco.Web.Models.Trees;
using Umbraco.Web.Mvc;
using Umbraco.Web.Trees;
using Umbraco.Web.WebApi.Filters;

namespace Diplo.TraceLogViewer.Controllers
{
	/// <summary>
	/// Diplo TraceLog Tree Controller
	/// </summary>
	/// <remarks>
	/// Creates the tree for the tracelogs, with each logfile as a separate node
	/// </remarks>
    [UmbracoApplicationAuthorize("developer")]
	[Tree(Constants.Applications.Developer, "diploTraceLog", "Trace Logs", sortOrder:9)]
	[PluginController("DiploTraceLogViewer")]
	public class TraceLogTreeController : TreeController
	{
		protected override TreeNodeCollection GetTreeNodes(string id, FormDataCollection queryStrings)
		{
			if (id != Constants.System.Root.ToInvariantString())
			{
				throw new HttpResponseException(HttpStatusCode.NotFound);
			}

			return PopulateTreeNodes(id, queryStrings);
		}

		protected override MenuItemCollection GetMenuForNode(string id, FormDataCollection queryStrings)
		{
			var menu = new MenuItemCollection();

			if (id == Constants.System.Root.ToInvariantString())
			{
				menu.Items.Add<RefreshNode, ActionRefresh>("Reload Log Files", true);
			}

			return menu;
		}

		private TreeNodeCollection PopulateTreeNodes(string parentId, FormDataCollection qs)
		{
			TreeNodeCollection tree = new TreeNodeCollection();
			LogFileService service = new LogFileService();

			foreach (var logFile in service.GetLogFiles())
			{
				string date = logFile.Date.ToString("yyyy-MM-dd");

				string path = HttpUtility.UrlEncode(System.IO.Path.GetFileName(logFile.Path));

				tree.Add(CreateTreeNode(path, parentId, qs, date, "icon-notepad"));
			}

			return tree;
		}
	}
}
