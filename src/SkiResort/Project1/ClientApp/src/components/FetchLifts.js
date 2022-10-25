import React, { Component } from 'react';

export class FetchLifts extends Component {
  static displayName = FetchLifts.name;

  constructor(props) {
    super(props);
    this.state = { lifts: [], loading: true };
  }

  componentDidMount() {
    this.populateLiftsData();
  }

  static renderLiftsTable(lifts) {
    return (
      <table className='table table-striped' aria-labelledby="tabelLabel">
        <thead>
          <tr>
            <th>liftName</th>
            <th>isOpen</th>
            <th>seatsAmounts</th>
            <th>liftingTime</th>
            <th>queueTime</th>
          </tr>
        </thead>
        <tbody>
          {lifts.map(lift =>
            <tr key={lift.liftName}>
              <td>{lift.isOpen}</td>
              <td>{lift.seatsAmounts}</td>
              <td>{lift.liftingTime}</td>
              <td>{lift.queueTime}</td>
            </tr>
          )}
        </tbody>
      </table>
    );
  }

  render() {
    let contents = this.state.loading
      ? <p><em>Loading...</em></p>
      : FetchLifts.renderLiftsTable(this.state.lifts);

    return (
      <div>
        <h1 id="tabelLabel" >Lifts</h1>
        <p>Test This component demonstrates fetching data from the server.</p>
        {contents}
      </div>
    );
  }

  async populateLiftsData() {
    const response = await fetch('lifts');
    const data = await response.json();
    this.setState({ lifts: data, loading: false });
  }
}
