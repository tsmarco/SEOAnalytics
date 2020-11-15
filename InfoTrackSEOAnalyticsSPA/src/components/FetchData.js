import React, { Component } from 'react';

export class FetchData extends Component {
  static displayName = FetchData.name;

  constructor(props) {
    super(props);
    this.state = { loading: true, searchTerm: "", tag: "", searchProvider: "google" };
    this.handleTagChange = this.handleTagChange.bind(this);
    this.handleSearchTermChange = this.handleSearchTermChange.bind(this);
    this.handleSearchProviderChange = this.handleSearchProviderChange.bind(this);
    this.getSearchData = this.getSearchData.bind(this);
  }

  handleTagChange(event) {
    this.setState({
      tag: event.target.value
    });
  }

  handleSearchTermChange(event) {
    this.setState({searchTerm: event.target.value});
  }

  handleSearchProviderChange(event) {
    this.setState({searchProvider: event.target.value});
  }

  async getSearchData() {
    const response = await fetch('https://localhost:44365/analytics?searchterm='+this.state.searchTerm+
    '&targettag='+this.state.tag +
    '&searchProvider='+this.state.searchProvider);
    const data = await response.json();
    this.setState({ searchResponse: data, loading: false });
  }

  static renderSearchResultTable(searchResponse) {
    return (
      <table className='table table-striped' aria-labelledby="tabelLabel">
        <thead>
          <tr>
            <th>Search Index Tag Appeared On</th>
 
          </tr>
        </thead>
        <tbody>
          {searchResponse.map(searchResponse =>
            <tr key={searchResponse}>
              <td>{searchResponse}</td>
            </tr>
          )}
        </tbody>
      </table>
    );
  }

  render() {
    let contents = this.state.loading
      ? <p><em></em></p>
      : FetchData.renderSearchResultTable(this.state.searchResponse);

    return (
      <div>
        <h1 id="tabelLabel" >Search Tag Analytics</h1>
        <p>This component lists the instances of a tag appearing in a search instance.</p>
        <label>
          Search Term: 
          <input type="text" value={this.state.searchTerm}  onChange={this.handleSearchTermChange} className=" ml-1 mr-4"/>
        </label>
        <label>
          Tag: 
          <input type="text" value={this.state.tag}  onChange={this.handleTagChange} className=" ml-1 mr-4"/>
        </label>
        <label>
          Search provider: 
          <select value={this.state.searchProvider} onChange={this.handleSearchProviderChange} className=" ml-1 mr-4">
            <option value="google">Google</option>
            <option value="bing">Bing</option>
          </select>
        </label>
        <button className="btn btn-primary" onClick={this.getSearchData}>Search</button>
        {contents}
      </div>
    );
  }

  
}
