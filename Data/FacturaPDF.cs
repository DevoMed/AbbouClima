using AbbouClima.Models;
using static System.Net.Mime.MediaTypeNames;
using System.Drawing;
using System;

namespace AbbouClima.Data
{
    public class FacturaPDF
    {
        public static string GenerarHtmlF(Presupuesto presupuesto)
        {
            string base64Image = ConvertImageToBase64("wwwroot/images/abbou.jpg");
            var html = $@"
<!DOCTYPE html>
<html lang=""es"">
<head>
    <meta charset=""UTF-8"">
    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
    <title>Presupuesto</title>
    <style>
.invoice-box {{
    max-width: 800px;
    margin: auto;
    padding: 30px;
    font-size: 20px;
    line-height: 24px;
    font-family: 'Helvetica Neue';
}}
.invoice-box table {{
    width: 100%;
    line-height: inherit;
    text-align: left;
    font-size: 20px;
}}
.invoice-box table td {{
    padding: 5px;
    vertical-align: top;
}}
 .invoice-box td:first-child{{
      width: 60%;
     text-align: left;
}}
.invoice-box td:not(:first-child){{
      width: 13.33%;
     text-align: center;
}}

.invoice-box table tr.top table td {{
    padding-bottom: 20px;
    font-size: 20px;
}}
.invoice-box table tr.top  td {{
    font-size: 20px;
}}
.invoice-box table tr.information  td {{
    font-size: 20px;
}}
.invoice-box table tr.top td {{
    padding-top: 20px;
    text-align: left;
}}
.invoice-box table tr.information table td {{
    padding-bottom: 40px;
    text-align: left;
    font-size: 20px;
}}
.invoice-box table tr.information td {{
    padding-bottom: 40px;
    text-align: left;
}}
.invoice-box table tr.heading td {{
    background: #eee;
    border-bottom: 1px solid #ddd;
    font-weight: bold;
}}
.invoice-box table tr.item td {{
    border-bottom: 1px solid #eee;
}}

.invoice-box .footer {{
    margin-top: 30px;
    text-align: center;
    font-weight: bold;
    color: #555;
}}
.invoice-box table tr.total td:not(:first-child) {{
    width: auto;
    text-align: right;
}}
.invoice-box tr.ultima td:first-child {{
    width: 25%;
    text-align: left;
    font-size: 20px;
}}
.invoice-box tr.ultima td:nth-child(2) {{
    width: 75%; 
    height:auto;
    text-align: left;
    padding-top: 10px;
    border-radius:8px;
  }}
  .invoice-box tr.top td span {{
        font-weight: bold;
  }}
    .invoice-box tr.information td span {{
         font-weight: bold;
  }}
    .invoice-box tr.total td span {{
         font-weight: bold;
  }}
    .invoice-box  tr.firma td {{
        width: 50%; 
        height:auto;
        font-weight: bold;
        text-align: left;
  }}


    </style>
</head>
<body>
    
<div class=""invoice-box"">
    <table cellpadding=""0"" cellspacing=""0"" >
        <tr class=""top"">
            <td>
                <div>
                  <img src=""data:image/jpeg;base64,{base64Image}"" alt=""Logo"" width=""150px"" height=""100px"">
                </div>
            </td>
            <td colspan=""3"">
                <span>Factura Nº:</span>{presupuesto.NºPresupuesto}
                <br> <span>Fecha:</span>{presupuesto.FechaPresupuesto}
            </td>

        </tr>
        <tr class=""information "">
            <td>
                <table>
                    <tr>
                        <td>
                            Abbou Clima<br>
                            C/ Manuel Galindo Nº 2<br>
                            28025 Madrid
                        </td>
                        <td>
                        </td>
                    </tr>
                </table>
            </td>
            <td colspan=""3"">
                {presupuesto.Cliente.Nombre}<br>
                {presupuesto.Cliente.Direccion}<br>
                {presupuesto.Cliente.Telefono}<br>
                {presupuesto.Cliente.Correo}<br>
                {presupuesto.Cliente.NIF}<br>                
            </td>
        </tr>
        <tr class=""heading "">
            <td>
                Descripción
            </td>
            <td>
                Cantidad
            </td>
            <td>
                Precio
            </td>
            <td>
                Total
            </td>
        </tr>

        <tr>
            <td>
                {presupuesto.Descripcion1}
            </td>
            <td>
                 {presupuesto.Cantidad1}
            </td>
            <td>
                {presupuesto.Precio1}
                <span class=""euro"" style=""visibility: hidden;"">€</span>         
            </td>
            <td>
                {presupuesto.Total1}
                <span class=""euro"" style=""visibility: hidden;"">€</span>  
            </td>
        </tr>
        <tr>
            <td>
                {presupuesto.Descripcion2}
            </td>
            <td>
                 {presupuesto.Cantidad2}
            </td>
            <td>
                 <span>{presupuesto.Precio2}</span> 
                 <span class=""euro"" style=""visibility: hidden;"">€</span>
            </td>
            <td>
                <span>{presupuesto.Total2}</span> 
                <span class=""euro"" style=""visibility: hidden;"">€</span>
            </td>
        </tr>
        <tr>
            <td>
                {presupuesto.Descripcion3}
            </td>
            <td>
                 {presupuesto.Cantidad3}
            </td>
            <td>
                <span> {presupuesto.Precio3}</span> 
                <span class=""euro"" style=""visibility: hidden;"">€</span>
            </td>
            <td>
                <span> {presupuesto.Total3}</span> 
                <span class=""euro"" style=""visibility: hidden;"">€</span>
            </td>
        </tr>
        <tr>
            <td>
                {presupuesto.Descripcion4}
            </td>
            <td>
                 {presupuesto.Cantidad4}
            </td>
            <td>
                <span> {presupuesto.Precio4}</span> 
                <span class=""euro"" style=""visibility: hidden;"">€</span>
            </td>
            <td>
                <span> {presupuesto.Total4}</span> 
                <span class=""euro"" style=""visibility: hidden;"">€</span>
            </td>
        </tr>
         <tr>
            <td>
                {presupuesto.Descripcion5}
            </td>
            <td>
                 {presupuesto.Cantidad5}
            </td>
            <td>
                <span> {presupuesto.Precio5}</span> 
                <span class=""euro"" style=""visibility: hidden;"">€</span>
            </td>
            <td>
                <span> {presupuesto.Total5}</span> 
                <span class=""euro"" style=""visibility: hidden;"">€</span>
            </td>
        </tr> 
         <tr>
            <td>
                {presupuesto.Descripcion6}
            </td>
            <td>
                 {presupuesto.Cantidad6}
            </td>
            <td>
                <span> {presupuesto.Precio6}<span>
                <span class=""euro"" style=""visibility: hidden;"">€</span>
            </td>
            <td>
                <span> {presupuesto.Total6}<span>
                <span class=""euro"" style=""visibility: hidden;"">€</span>
            </td>
        </tr>
         <tr>
            <td>
                {presupuesto.Descripcion7}
            </td>
            <td>
                 {presupuesto.Cantidad7}
            </td>
            <td>
                <span> {presupuesto.Precio7}<span>
                <span class=""euro"" style=""visibility: hidden;"">€</span>
            </td>
            <td>
                <span> {presupuesto.Total7}<span>
                <span class=""euro"" style=""visibility: hidden;"">€</span>
            </td>
        </tr>
        <tr>
            <td>
                {presupuesto.Descripcion8}
            </td>
            <td>
                 {presupuesto.Cantidad8}
            </td>
            <td>
                <span> {presupuesto.Precio8}<span>
                <span class=""euro"" style=""visibility: hidden;"">€</span>
            </td>
            <td>
                <span> {presupuesto.Total8}<span>
                <span class=""euro"" style=""visibility: hidden;"">€</span>
            </td>
        </tr>
        <tr>
            <td>
                {presupuesto.Descripcion9}
            </td>
            <td>
                 {presupuesto.Cantidad9}
            </td>
            <td>
                <span> {presupuesto.Precio9}<span>
                <span class=""euro"" style=""visibility: hidden;"">€</span>
            </td>
            <td>
                <span> {presupuesto.Total9}<span>
                <span class=""euro"" style=""visibility: hidden;"">€</span>
            </td>
        </tr>
         <tr>
            <td>
                {presupuesto.Descripcion10}
            </td>
            <td>
                 {presupuesto.Cantidad10}
            </td>
            <td>
                <span> {presupuesto.Precio10}<span>
                    <span class=""euro"" style=""visibility: hidden;"">€</span>
            </td>
            <td>
                <span> {presupuesto.Total10}<span>
                <span class=""euro"" style=""visibility: hidden;"">€</span>
            </td>
        </tr>
        </table>
        <hr>
        <table>
        <tr class=""ultima"">
            <td class=""item"">Observaciones:</td>
            <td colspan=""3"">{presupuesto.Observaciones}</td>
        </tr>
        </table>
        <hr>
        <table >                                                   
        <tr class=""total"">
            <td></td>          
            <td>
                <span>Total sin IVA:</span> {presupuesto.TotalSinIVA} €
            </td>
             <td></td>
        </tr>
         <tr class=""total"">
            <td></td>           
            <td>
                <span>Total:</span> {presupuesto.ImporteTotal} €
            </td>
             <td></td>
        </tr>       
    </table>
    <hr>  
    <table>                                                   
        <tr class=""firma"">
            <td><span>Firma proveedor:</span> </td>          
            <td colspan=""3""><span>Firma cliente:</span></td>
        </tr>      
    </table>

</div>

   
</body>

</html>  ";
            return html;
        }
        public static string ConvertImageToBase64(string imagePath)
        {
            byte[] imageBytes = File.ReadAllBytes(imagePath);
            return Convert.ToBase64String(imageBytes);
        }
    }
}
