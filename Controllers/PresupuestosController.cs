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
using Microsoft.Extensions.Options;
using jsreport.Local;
using jsreport.Binary;
using jsreport.Types;

namespace AbbouClima.Controllers
{
    public class PresupuestosController : Controller
    {
        private readonly IPresupuestoService _presupuestoService;
        private readonly AppDbContext _context;
        private readonly GlobalSettings _globalSettings;
        private string RutaCarpetaPdf;
        private string RutaCarpetaFactura;
        private IConfiguration _configuration;
        private readonly WriterLog _logger;

        public PresupuestosController(AppDbContext context, IOptions<GlobalSettings> globalSettings, IPresupuestoService presupuestoService, IConfiguration configuration)
        {
            _context = context;
            _globalSettings = globalSettings.Value;
            RutaCarpetaPdf = _globalSettings.RutaCarpetaPdf;
            RutaCarpetaFactura = _globalSettings.RutaCarpetaFactura;
            _presupuestoService = presupuestoService;
            _configuration = configuration;
            string logFolder = globalSettings.Value.LogFolder;
            _logger = WriterLog.Instance(logFolder);
        }
        // Método para mostrar el formulario de presupuesto
        public IActionResult Create()
        {
            var modelo = new Presupuesto();
            try
            {
            var listado = _context.Clientes.Where(l => l.Borrado == false).ToList();
            var clientes = listado.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Nombre
            }).ToList();

            ViewBag.Clientes = clientes;

            }
            catch (Exception ex)
            {

                _logger.Log(ex.Message.ToString());
            }
            return View(modelo);
        }

        // Método para crear el presupuesto y generar el PDF
        [HttpPost]
        public async Task<IActionResult> Create(Presupuesto presupuesto)
        {
            try
            {
            if (ModelState.IsValid)
            {
                await _presupuestoService.CreatePresupuesto(presupuesto);
                return await GenerarPDF(presupuesto);
            }
            }
            catch (Exception ex)
            {
                _logger.Log(ex.Message.ToString());
            }
            return View(presupuesto);
        }

        public async Task<IActionResult> Index(int? id, string busqueda)
        {
            List<Presupuesto> presupuestos = new List<Presupuesto>();
            try
            {
             presupuestos = await _presupuestoService.GetAll(id, busqueda);
            }
            catch (Exception ex)
            {
                _logger.Log(ex.Message.ToString());
            }
            return View(presupuestos);

        }
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
            var presupuesto = await _presupuestoService.CdetailsById(id);
            }
            catch (Exception ex)
            {
                _logger.Log(ex.Message.ToString());
            }
            return RedirectToAction(nameof(Index), new { id = id });
        }

        // GET: Presupuestos/Details/5
        public async Task<IActionResult> Details(Guid id)
        {
            Presupuesto presupuesto = new Presupuesto();
            try
            {
             presupuesto = await _presupuestoService.PdetailsById(id);

            if (presupuesto == null)
            {
                return NotFound();
            }
            }
            catch (Exception ex)
            {
                _logger.Log(ex.Message.ToString());
            }
            return View(presupuesto);
        }

        // GET: Presupuestos/Edit/5
        public async Task<IActionResult> Edit(Guid id)
        {
            Presupuesto presupuesto = new Presupuesto();
            try
            {
                presupuesto = await _context.Presupuestos.Include(p => p.Cliente).FirstOrDefaultAsync(m => m.Id == id);

                if (presupuesto == null)
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                _logger.Log(ex.Message.ToString());
            }
            return View(presupuesto);
        }

        // POST: Presupuestos/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, Presupuesto presupuesto)
        {
            
			if (id != presupuesto.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _presupuestoService.EditPresupuesto(id, presupuesto);
                }
                catch (DbUpdateConcurrencyException Exception )
                {
                    if (!PresupuestoExists(presupuesto.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        _logger.Log(Exception.Message.ToString());
                    }
                }
                return await GenerarPDF(presupuesto);
            }
            return View(presupuesto);
        }

        // GET: Presupuestos/Delete/5
        public async Task<IActionResult> Delete(Guid id)
        {
            Presupuesto presupuesto = new Presupuesto();
            try
            {
             presupuesto = await _context.Presupuestos.FirstOrDefaultAsync(m => m.Id == id);

            if (presupuesto == null)
            {
                return NotFound();
            }
            }
            catch (Exception ex)
            {
                _logger.Log(ex.Message.ToString());
            }
            return View(presupuesto);
        }

        // POST: Presupuestos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            Presupuesto presupuesto = new Presupuesto();
            try
            {
             presupuesto = await _context.Presupuestos.Include(p => p.Cliente).FirstOrDefaultAsync(m => m.Id == id);

            if (presupuesto != null)
            {
                _context.Presupuestos.Remove(presupuesto);
            }
            await _context.SaveChangesAsync();

            var carpetaPdf = Path.Combine($"{RutaCarpetaPdf}{presupuesto.Cliente.Nombre}");
            var nombrePdf = $"{presupuesto.Cliente.Nombre}-{presupuesto.NºPresupuesto}.pdf";
            var rutaArchivo = Path.Combine(carpetaPdf, nombrePdf);

            if (System.IO.File.Exists(rutaArchivo))
            {
                System.IO.File.Delete(rutaArchivo);
            }
            else
            {
                return NotFound(new { mensaje = $"El archivo {nombrePdf} no fue encontrado." });
            }
            }
            catch (Exception ex)
            {
                _logger.Log(ex.Message.ToString());
            }
            return RedirectToAction(nameof(Index));
        }

        private bool PresupuestoExists(Guid id)
        {
            return _context.Presupuestos.Any(e => e.Id == id);
        }
        public async Task<IActionResult> GenerarPDF(Presupuesto presupuesto)
        {
            try
            {

            // Crear el contenido HTML dinámico
            var htmlContent = PresupuestoPDF.GenerarHtml(presupuesto);

            // Inicializar jsreport como un servidor local
            var rs = new LocalReporting()
                .UseBinary(JsReportBinary.GetBinary()) // Cargar binario
                .AsUtility()
                .Create();

            // Configurar la solicitud de renderizado
            var renderRequest = new RenderRequest
            {
                Template = new Template
                {
                    Content = htmlContent,
                    Engine = Engine.None,
                    Recipe = Recipe.ChromePdf
                }
            };

            // Generar el reporte
            var report = await rs.RenderAsync(renderRequest);
            if (report == null)
            {
                TempData["ErrorMessage"] = "Hubo un error al generar el PDF.";
                return View(presupuesto);
            }
            using (var memoryStream = new MemoryStream())
            {
                report.Content.CopyTo(memoryStream); // Copiar el Stream al MemoryStream
                var pdfBytes = memoryStream.ToArray(); // Convertir a byte[]

                var nombrePdf = $"{presupuesto.Cliente.Nombre}-{presupuesto.NºPresupuesto}.pdf";
                //var RutaCarpetaPdf = _globalSettings.RutaCarpetaPdf;
                var carpetaPdf = Path.Combine($"{RutaCarpetaPdf}{presupuesto.Cliente.Nombre}");

                if (!Directory.Exists(carpetaPdf))
                {
                    Directory.CreateDirectory(carpetaPdf);
                }
                var rutaPdf = Path.Combine(carpetaPdf, nombrePdf);

                System.IO.File.WriteAllBytes(rutaPdf, pdfBytes);

                var fileUrl = Url.Action("AbrirPDF", "Presupuestos", new { filePath = rutaPdf });
                TempData["PDFUrl"] = fileUrl;
                TempData["SuccessMessage"] = "El PDF se generó y se guardó correctamente.";              
            }
            }
            catch (Exception ex)
            {
                _logger.Log(ex.Message.ToString());
            }
            return RedirectToAction(nameof(Index));
        }
        public IActionResult AbrirPDF(string filePath)
        {
            try
            {
                if (string.IsNullOrEmpty(filePath) || !System.IO.File.Exists(filePath))
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                _logger.Log(ex.Message.ToString());
            }
            var fileBytes = System.IO.File.ReadAllBytes(filePath);
            return File(fileBytes, "application/pdf");
        }

        public IActionResult AbrirPDFDetalles(string filePath)
        {
           var  RutaPdf = Path.Combine($"{RutaCarpetaPdf}{filePath}.pdf");
            try
            {
            if (string.IsNullOrEmpty(RutaPdf) || !System.IO.File.Exists(RutaPdf))
            {
                return NotFound();
            }
            }
            catch (Exception ex)
            {
                _logger.Log(ex.Message.ToString());
            }

            var fileBytes = System.IO.File.ReadAllBytes(RutaPdf);
            return File(fileBytes, "application/pdf");
        }

        public async Task<IActionResult> EnviarPresupuesto(Guid id)
        {
            try
            {
            // Buscar el presupuesto en la base de datos
            var presupuesto = await _context.Presupuestos
                .Include(p => p.Cliente)
                .FirstOrDefaultAsync(p => p.Id == id);
            if (presupuesto.Cliente.Correo != null)
            {
                if (presupuesto == null)
                {
                    // Redirigir a la página de error si no se encuentra el presupuesto
                    return RedirectToAction("Error", "Home");
                }

                // Ruta del archivo PDF
                var rutaBase = Path.Combine($"{RutaCarpetaPdf}{presupuesto.Cliente.Nombre}");
                var nombreArchivo = $"{presupuesto.Cliente.Nombre}-{presupuesto.NºPresupuesto}.pdf";
                var rutaArchivo = Path.Combine(rutaBase, nombreArchivo);

                if (!System.IO.File.Exists(rutaArchivo))
                {
                    // Redirigir a la página de error si el archivo PDF no existe
                    return RedirectToAction("Error", "Home");
                }

                // Leer el archivo como array de bytes
                byte[] archivoPdf = await System.IO.File.ReadAllBytesAsync(rutaArchivo);

                // Preparar el correo
                var emailService = new EmailService(_configuration);
                var asunto = nombreArchivo;
                var mensaje = $"Estimado {presupuesto.Cliente.Nombre},<br><br>Adjunto encontrará su presupuesto.<br><br>Gracias.";
                await emailService.EnviarPresupuestoAsync(presupuesto.Cliente.Correo, asunto, mensaje, archivoPdf, nombreArchivo);

                presupuesto.Enviado = true;
                _context.Presupuestos.Update(presupuesto);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "El email se ha enviado correctamente.";
            }else
            {
				TempData["ErrorMessage"] = "Este cliente no tiene ningún email asignado";
			}

            }
            catch (Exception ex)
            {
                _logger.Log(ex.Message.ToString());
            }
            return RedirectToAction(nameof(Details), new { id = id });
		}
        public async Task<IActionResult> GenerarFactura(Guid id)
        {
            try
            {
            var presupuesto = await _context.Presupuestos .Include(p => p.Cliente).FirstOrDefaultAsync(p => p.Id == id);
            presupuesto.FechaPresupuesto = DateTime.Now.ToString("dd/MM/yyyy");
            presupuesto.ImporteTotal = (decimal?)(presupuesto.TotalSinIVA * (1.21));

            if (presupuesto != null)
            {
                // Crear el contenido HTML dinámico
                var htmlContent = FacturaPDF.GenerarHtmlF(presupuesto);

                // Inicializar jsreport como un servidor local
                var rs = new LocalReporting()
                    .UseBinary(JsReportBinary.GetBinary()) // Cargar binario
                    .AsUtility()
                    .Create();

                // Configurar la solicitud de renderizado
                var renderRequest = new RenderRequest
                {
                    Template = new Template
                    {
                        Content = htmlContent,
                        Engine = Engine.None,
                        Recipe = Recipe.ChromePdf
                    }
                };              
                // Generar el reporte
                var report = await rs.RenderAsync(renderRequest);



                if (report == null)
                {
                    TempData["ErrorMessage"] = "Hubo un error al generar el PDF.";
                    return View(presupuesto);
                }
                using (var memoryStream = new MemoryStream())
                {

                    report.Content.CopyTo(memoryStream); // Copiar el Stream al MemoryStream
                    var pdfBytes = memoryStream.ToArray(); // Convertir a byte[]

                    var nombreFactura = $"{presupuesto.Cliente.Nombre}-{presupuesto.NºPresupuesto}.pdf";
                    //var RutaCarpetaPdf = _globalSettings.RutaCarpetaPdf;
                    var carpetaFactura = Path.Combine($"{RutaCarpetaFactura}{presupuesto.Cliente.Nombre}");

                    if (!Directory.Exists(carpetaFactura))
                    {
                        Directory.CreateDirectory(carpetaFactura);
                    }
                    var rutaPdf = Path.Combine(carpetaFactura, nombreFactura);

                    System.IO.File.WriteAllBytes(rutaPdf, pdfBytes);

                    var fileUrl = Url.Action("AbrirPDF", "Presupuestos", new { filePath = rutaPdf });
                    TempData["PDFUrl"] = fileUrl;
                    TempData["SuccessMessage"] = "La factura se generó y se guardó correctamente.";

                    return RedirectToAction(nameof(Details), new { id = id });

                }
                
            }

            }
            catch (Exception ex)
            {
                _logger.Log(ex.Message.ToString());
            }
            return RedirectToAction("Error", "Home");
        }


    }
}
