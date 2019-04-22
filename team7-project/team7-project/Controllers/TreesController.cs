﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Models.Trees.Services;
using Models.Trees;
using team7_project.Errors;
using Models.Converters.Trees;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace team7_project.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class TreesController : Controller
    {
        private readonly ITreeService trees;

        public TreesController(ITreeService trees)
        {
            this.trees = trees;
        }

        /// <summary>
        /// Возвращает дерево по id
        /// </summary> 
        /// <param name="treeId"> </param> 
        /// <returns>Дерево</returns>
        /// <response code="201">Возвращает дерево</response>
        /// <response code="400">Если дерево по указанному индексу отсутствует</response>  
        [AllowAnonymous]
        [HttpGet]
        [ProducesResponseType(typeof(Client.Models.Trees.Tree),201)]
        [ProducesResponseType(400)]
        [Route("{treeId}", Name = "GetTreeRoute")]
        public async Task<IActionResult> GetTreeAsync([FromRoute] string treeId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            Models.Trees.Tree tree;
            try
            {
                tree = await trees.GetAsync(treeId, cancellationToken);
            }
            catch (TreeNotFoundException)
            {
                var error = ServiceErrorResponses.TreeNotFound(treeId);
                return BadRequest();
            }

            var clientTree = TreeConverter.Convert(tree);

            return Ok(clientTree);
        }

        /// <summary>
        /// Возвращает список из всех деревьев
        /// </summary>    
        /// <returns>Список информации о деревьях</returns>
        /// <response code="201">Возвращает список информации о деревьях</response>
        [AllowAnonymous]
        [HttpGet]
        [ProducesResponseType(typeof(IReadOnlyList<TreeInfo>), 201)]
        public async Task<IActionResult> GetTreeAsync(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var info = await trees.GetAllAsync(cancellationToken);

            return Ok(info);
        }

        /// <summary>
        /// Создание дерева
        /// </summary>     
        /// <remarks>
        /// В процессе разработки
        /// </remarks>  
        /// <returns>Id дерева</returns>
        /// <response code="201">Возвращает Id созданного дерева</response>
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        [ProducesResponseType(typeof(string), 201)]
        [Route("create")]
        public async Task<IActionResult> CreateTreeAsync([FromBody] Client.Models.Trees.TreeCreationInfo treeCreationInfo, CancellationToken cancellationToken)
        {
            Models.Trees.TreeCreationInfo tree = null;
            var treeId = await trees.CreateAsync(tree, cancellationToken);

            return Ok(treeId);
        }
    }
}