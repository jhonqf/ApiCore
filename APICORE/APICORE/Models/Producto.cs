﻿namespace APICORE.Models
{
    public class Producto
    {
        public int IdProducto { get; set; }
        public string CodigoBArra { get; set;}
        public string Nombre { get; set;}
        public string Marca { get; set;}
        public string Categoria { get; set;}
        public decimal Precio { get; set;}
    }
}