using CommonArchitecture.Application.Commands.Menus.CreateMenu;
using CommonArchitecture.Application.Commands.Menus.DeleteMenu;
using CommonArchitecture.Application.Commands.Menus.UpdateMenu;
using CommonArchitecture.Application.DTOs;
using CommonArchitecture.Application.Queries.Menus.GetAllMenus;
using CommonArchitecture.Application.Queries.Menus.GetMenuById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CommonArchitecture.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MenusController : ControllerBase
{
 private readonly IMediator _mediator;

 public MenusController(IMediator mediator)
 {
 _mediator = mediator;
 }

 [HttpGet]
 [AllowAnonymous]
 public async Task<ActionResult<PaginatedResult<MenuDto>>> GetAll([FromQuery] MenuQueryParameters parameters)
 {
 var query = new GetAllMenusQuery(parameters);
 var result = await _mediator.Send(query);
 return Ok(result);
 }

 [HttpGet("{id}")]
 [AllowAnonymous]
 public async Task<ActionResult<MenuDto>> GetById(int id)
 {
 var query = new GetMenuByIdQuery(id);
 var menu = await _mediator.Send(query);
 
 if (menu == null)
 return NotFound();

 return Ok(menu);
 }

 [HttpPost]
 [Authorize]
 public async Task<ActionResult<MenuDto>> Create(CreateMenuDto createDto)
 {
 var command = new CreateMenuCommand(
 createDto.Name,
 createDto.Icon,
 createDto.Url,
 createDto.ParentMenuId,
 createDto.DisplayOrder
 );
 
 var menu = await _mediator.Send(command);
 return CreatedAtAction(nameof(GetById), new { id = menu.Id }, menu);
 }

 [HttpPut("{id}")]
 [Authorize]
 public async Task<IActionResult> Update(int id, UpdateMenuDto updateDto)
 {
 var command = new UpdateMenuCommand(
 id,
 updateDto.Name,
 updateDto.Icon,
 updateDto.Url,
 updateDto.ParentMenuId,
 updateDto.DisplayOrder,
 updateDto.IsActive
 );
 
 var result = await _mediator.Send(command);
 if (!result)
 return NotFound();

 return NoContent();
 }

 [HttpDelete("{id}")]
 [Authorize]
 public async Task<IActionResult> Delete(int id)
 {
 var command = new DeleteMenuCommand(id);
 var result = await _mediator.Send(command);
 
 if (!result)
 return NotFound();

 return NoContent();
 }
}
