using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AbbouClima.Data;
using AbbouClima.Models;
using AbbouClima.Services;
using AbbouClima.Interfaces;

namespace AbbouClima.Controllers
{
    public class ClientesController : Controller
    {
        private readonly AppDbContext _context;

        private readonly IClienteService _clienteService;

        public ClientesController(AppDbContext context, IClienteService clienteService)
        {
            _context = context;
            _clienteService = clienteService;
        }

        // GET: Clientes
        public async Task<IActionResult> Index(string busqueda)
        {
            var clientes = await _clienteService.GetAll(busqueda);

            return View(clientes);
        }

        // GET: Clientes/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var cliente = await _clienteService.DetailsById(id);

            if (cliente == null)
            {
                return NotFound();
            }

            return View(cliente);
        }

        // GET: Clientes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Clientes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nombre,NIF,Correo,Telefono,Direccion,FechaRegistro")] Cliente cliente)
        {
            if (ModelState.IsValid)
            {
                await _clienteService.CreateCliente(cliente);
                return RedirectToAction(nameof(Index));
            }
            return View(cliente);
        }

        // GET: Clientes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // var cliente = await _context.Clientes.FindAsync(id);
            var cliente = await _clienteService.DetailsById(id);
            if (cliente == null)
            {
                return NotFound();
            }
            return View(cliente);
        }

        // POST: Clientes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre,NIF,Correo,Telefono,Direccion,FechaRegistro")] Cliente cliente)
        {
            if (id != cliente.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {

                    await _clienteService.EditClient(cliente);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_clienteService.ClienteExists(cliente.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(cliente);
        }

        // GET: Clientes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //var cliente = await _context.Clientes.FirstOrDefaultAsync(m => m.Id == id);
            var cliente = await _clienteService.DetailsById(id);
            if (cliente == null)
            {
                return NotFound();
            }

            return View(cliente);
        }

        // POST: Clientes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cliente = await _clienteService.DetailsById(id);
            //var cliente = await _context.Clientes.FindAsync(id);
            if (cliente != null)
            {
                //_context.Clientes.Remove(cliente);

                await _clienteService.DeleteClient(cliente);
            }

            return RedirectToAction(nameof(Index));
        }

       
    }
}
