// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ClientScopeDto" company="">
//
// </copyright>
// <summary>
//   The class ClientScopeDto.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace AdminApi.V1.Dtos
{
    #region Usings

    using System;

    #endregion

    /// <summary>
    ///     The ClientScopeDto.
    /// </summary>
    public class ClientScopeDto
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ClientScopeDto"/> class.
        /// </summary>
        public ClientScopeDto()
        {
        }

        #endregion

        #region Public Properties

        public int Id { get; set; }
        public string Scope { get; set; }
        public int ClientId { get; set; }

        #endregion

        #region Public Methods And Operators


        #endregion

        #region Other Methods

        #endregion
    }
}