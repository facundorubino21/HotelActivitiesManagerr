﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dominio;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AppWeb.Controllers
{
    public class UsuarioController : Controller
    {
        private Sistema _sistema = Sistema.Instancia;

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string email, string pass)
        {
            try
            {
                Usuario usuarioLogueado = _sistema.ObtenerUsuario(email, pass);
                string rol = _sistema.ObtenerRolUsuario(email, pass);

                HttpContext.Session.SetString("email", email);
                HttpContext.Session.SetString("rol", rol);

                if (rol == "huesped")
                    return Redirect("/Actividad/Index");
                else if (rol == "operador")
                    return Redirect("/Proveedor/Index");
            }
            catch (Exception e)
            {
                ViewBag.error = e.Message;
            }
            return View("login");

        }

        public IActionResult LogOut()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("login");
        }

        [HttpGet]
        public IActionResult CreateHuesped()
        {
            ViewBag.TiposDocumento = _sistema.TiposDeDocumento;
            return View(new Huesped());
        }

        [HttpPost]
        public IActionResult CreateHuesped(Huesped huesped, int tipoDocumento)
        {
            try
            {
                // alta en sistema
                _sistema.AgregarHuesped(huesped);
                // return RedirectToAction("MostrarUsuario", new { mensaje = "Registrado correctamente" });
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
                ViewBag.error = e.Message;
            }
            return View("index");
        }

        [HttpGet]
        public IActionResult MostrarUsuario()
        {
            return View("MuestroUsuario");
        }
    }
}

