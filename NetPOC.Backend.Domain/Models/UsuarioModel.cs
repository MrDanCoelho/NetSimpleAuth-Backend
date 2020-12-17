using System;
using System.ComponentModel.DataAnnotations;
using NetPOC.Backend.Domain.Enums;
using NetPOC.Backend.Domain.Validations;

namespace NetPOC.Backend.Domain.Models
{
    public class UsuarioModel
    {
        [Key]
        public int Id { get; set; }
        
        [Required(ErrorMessage = "É necessário informar um Nome")]
        public string Nome { get; set; }
        
        [Required(ErrorMessage = "É necessário informar um Sobrenome")]
        public string Sobrenome { get; set; }
        
        [Required(ErrorMessage = "É necessário informar um E-mail")]
        [EmailAddress(ErrorMessage = "E-mail não é válido")]
        public string Email { get; set; }
        
        [Required(ErrorMessage = "É necessário informar uma Data de Nascimento")]
        [CheckDataNascimento]
        public DateTime DataNascimento { get; set; }
        
        [Required(ErrorMessage = "É necessário informar a Escolaridade")]
        [EnumDataType(typeof(FlagEscolaridade), ErrorMessage = "Este id de escolaridade não é válido")]
        public FlagEscolaridade Escolaridade { get; set; }
    }
}