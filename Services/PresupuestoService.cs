using AbbouClima.Data;
using AbbouClima.Interfaces;
using AbbouClima.Models;
using jsreport.Binary;
using jsreport.Local;
using jsreport.Types;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Web.Mvc;

namespace AbbouClima.Services
{
    public class PresupuestoService : IPresupuestoService
    {
        private readonly AppDbContext _context;
        public PresupuestoService(AppDbContext context)
        {
            _context = context;
        }

        public async Task CreatePresupuesto(Presupuesto presupuesto)
        {
            Random random = new Random();
            const decimal iva = 0.21m;
            decimal? totalSinIVA = presupuesto.TotalSinIVA;
            decimal? totalConIVA = totalSinIVA;

                var cliente = _context.Clientes.FirstOrDefault(c => c.Id == presupuesto.ClienteId);

                if (cliente != null)
                {
                    presupuesto.Cliente = cliente;
                }
                if (presupuesto.IncludeIVA)
                {
                    totalConIVA = totalSinIVA * (1 + iva);
                }
                if (presupuesto.Validez == null)
                {
                    presupuesto.Validez = 30;
                }

                presupuesto.ImporteTotal = totalConIVA;
                presupuesto.Enviado = false;
                presupuesto.NºPresupuesto = DateTime.Now.ToString("yyyyMM") + random.Next(1000, 9999);
                presupuesto.FechaPresupuesto = DateTime.Now.ToString("dd/MM/yyyy");

                _context.Presupuestos.Add(presupuesto);
               await _context.SaveChangesAsync();
        }

        public Task<Presupuesto> CdetailsById(int id)
        {
            var presupuesto =  _context.Presupuestos.FirstOrDefaultAsync(m => m.ClienteId == id);

             return presupuesto;          
        }
        public  Task<Presupuesto> PdetailsById(Guid id)
        {
            var presupuesto =  _context.Presupuestos.Include(p => p.Cliente)
                                                         .FirstOrDefaultAsync(m => m.Id == id);
            return presupuesto;
        }
        public async Task EditPresupuesto(Guid id, Presupuesto presupuesto)
        {
            const decimal iva = 0.21m;
            var cliente = await _context.Clientes.FirstOrDefaultAsync(c => c.Id == presupuesto.ClienteId);

            if (cliente != null)
            {
                presupuesto.Cliente = cliente;
            }
{
                if (presupuesto.IncludeIVA)
                {
                    presupuesto.ImporteTotal = presupuesto.TotalSinIVA * (1 + iva);
                }
                else presupuesto.ImporteTotal = presupuesto.TotalSinIVA;

                if (presupuesto.Validez == null)
                {
                    presupuesto.Validez = 30;
                }

                presupuesto.Enviado = false;
                _context.Update(presupuesto);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<Presupuesto>> GetAll(int? id, string busqueda)
        {

            var presupuesto = new List<Presupuesto>();

            if (id == null & string.IsNullOrEmpty(busqueda))
            {
                presupuesto = await _context.Presupuestos.Include(p => p.Cliente).OrderByDescending(p => p.FechaPresupuesto).ToListAsync();

            }
             if (id != null & string.IsNullOrEmpty(busqueda))
            {
                presupuesto = await _context.Presupuestos.Include(p => p.Cliente).Where(m => m.ClienteId.Equals(id)).OrderByDescending(p => p.FechaPresupuesto).ToListAsync();

            }
            if (!string.IsNullOrEmpty(busqueda))
            {
                presupuesto = await _context.Presupuestos.Include(p => p.Cliente).Where(m => m.NºPresupuesto.Contains(busqueda)).ToListAsync();

            }
            return (presupuesto);

        }

    }
}
