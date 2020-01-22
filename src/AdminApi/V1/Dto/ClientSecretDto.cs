// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ClientSecretDto" company="">
//
// </copyright>
// <summary>
//   The class ClientSecretDto.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace AdminApi.V1.Dtos
{
    #region Usings

    using System;

    #endregion

    /// <summary>
    ///     The ClientSecretDto.
    /// </summary>
    public class ClientSecretDto
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ClientSecretDto"/> class.
        /// </summary>
        public ClientSecretDto()
        {
        }

        #endregion

        #region Public Properties

        public int Id { get; set; }
        public int ClientId { get; set; }
        public string Description { get; set; }
        public string Value { get; set; }
        public DateTime? Expiration { get; set; }
        public string Type { get; set; }
        public DateTime Created { get; set; }

        #endregion

        #region Public Methods And Operators


        #endregion

        #region Other Methods

        #endregion
    }
}