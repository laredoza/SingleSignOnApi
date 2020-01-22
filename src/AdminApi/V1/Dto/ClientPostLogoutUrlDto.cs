// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ClientPostLogoutUrlDto" company="">
//
// </copyright>
// <summary>
//   The class ClientPostLogoutUrlDto.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace AdminApi.V1.Dtos
{
    #region Usings

    using System;

    #endregion

    /// <summary>
    ///     The ClientRedirectUrlDto.
    /// </summary>
    public class ClientPostLogoutUrlDto
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ClientPostLogoutUrlDto"/> class.
        /// </summary>
        public ClientPostLogoutUrlDto()
        {
        }

        #endregion

        #region Public Properties

        public int Id { get; set; }
        public string PostLogoutRedirectUri { get; set; }
        public int ClientId { get; set; }

        #endregion

        #region Public Methods And Operators


        #endregion

        #region Other Methods

        #endregion
    }
}