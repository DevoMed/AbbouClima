using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AbbouClima.Data;
using AbbouClima.Models;
using Microsoft.EntityFrameworkCore;
using System.Web.WebPages.Html;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Ajax.Utilities;
using AbbouClima.Interfaces;

namespace AbbouClima.Services
{
    public class ClienteService : IClienteService
    {
        private readonly AppDbContext _context;
        public ClienteService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<List<Cliente>> GetAll(string busqueda) {

            if (!string.IsNullOrEmpty(busqueda))
            {
                return await _context.Clientes.Where(c => c.Borrado == false & c.Nombre.ToLower().Contains(busqueda.Trim().ToLower())).ToListAsync();
            }

            return await _context.Clientes.Where(c => c.Borrado == false).ToListAsync();

        }

        public async Task<Cliente> DetailsById(int? id)
        {
            var client = await _context.Clientes.FirstOrDefaultAsync(m => m.Id == id);

            return client;
        }

        public async Task CreateCliente(Cliente cliente)
        {
            cliente.FechaRegistro = DateTime.Now.ToString("dd-MM-yyyy");
            cliente.FechaModificacion = cliente.FechaRegistro;
            _context.Add(cliente);
            await _context.SaveChangesAsync();
           
        }
        public async Task EditClient(Cliente cliente)
        {
            cliente.FechaModificacion = DateTime.Now.ToString("dd-MM-yyyy");          
            _context.Update(cliente);
            await _context.SaveChangesAsync();

        }

        public async Task DeleteClient(Cliente cliente)
        {
            cliente.Borrado = true;
            cliente.FechaModificacion = DateTime.Now.ToString("dd-MM-yyyy");
            await _context.SaveChangesAsync();

        }
        public bool ClienteExists(int id)
        {
            return _context.Clientes.Any(e => e.Id == id);
        }
    }
}
