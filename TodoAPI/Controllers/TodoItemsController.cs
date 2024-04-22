﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TodoAPI.Interfaces;
using TodoAPI.Models;

namespace TodoAPI.Controllers;

#region snippetErrorCode
public enum ErrorCode
{
    TodoItemNameAndNotesRequired,
    TodoItemIDInUse,
    RecordNotFound,
    CouldNotCreateItem,
    CouldNotUpdateItem,
    CouldNotDeleteItem
}
#endregion

#region snippetDI
[ApiController]
[Authorize]
[Route("api/[controller]")]

public class TodoItemsController(ITodoRepository todoRepository) : ControllerBase
{
    #endregion

    #region snippet
    [HttpGet]
    [AllowAnonymous]
    public IActionResult List()
    {
        return Ok(todoRepository.All);
    }
    #endregion

    #region snippetCreate
    [HttpPost]
    public IActionResult Create([FromBody]TodoItem item)
    {
        try
        {
            if (item == null || !ModelState.IsValid)
            {
                return BadRequest(ErrorCode.TodoItemNameAndNotesRequired.ToString());
            }
            bool itemExists = todoRepository.DoesItemExist(item.ID);
            if (itemExists)
            {
                return StatusCode(StatusCodes.Status409Conflict, ErrorCode.TodoItemIDInUse.ToString());
            }
            todoRepository.Insert(item);
        }
        catch (Exception)
        {
            return BadRequest(ErrorCode.CouldNotCreateItem.ToString());
        }
        return Ok(item);
    }
    #endregion

    #region snippetEdit
    [HttpPut]
    public IActionResult Edit([FromBody] TodoItem item)
    {
        try
        {
            if (item == null || !ModelState.IsValid)
            {
                return BadRequest(ErrorCode.TodoItemNameAndNotesRequired.ToString());
            }
            var existingItem = todoRepository.Find(item.ID);
            if (existingItem == null)
            {
                return NotFound(ErrorCode.RecordNotFound.ToString());
            }
            todoRepository.Update(item);
        }
        catch (Exception)
        {
            return BadRequest(ErrorCode.CouldNotUpdateItem.ToString());
        }
        return NoContent();
    }
    #endregion
    
    #region snippetDelete
    [HttpDelete("{id}")]
    public IActionResult Delete(string id)
    {
        try
        {
            var item = todoRepository.Find(id);
            if (item == null)
            {
                return NotFound(ErrorCode.RecordNotFound.ToString());
            }
            todoRepository.Delete(id);
        }
        catch (Exception)
        {
            return BadRequest(ErrorCode.CouldNotDeleteItem.ToString());
        }
        return NoContent();
    }
    #endregion
}
