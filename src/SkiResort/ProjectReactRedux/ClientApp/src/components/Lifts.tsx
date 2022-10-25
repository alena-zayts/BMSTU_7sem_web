import * as React from 'react';
import { connect } from 'react-redux';
import { RouteComponentProps } from 'react-router';
import { Link } from 'react-router-dom';
import { ApplicationState } from '../store';
import * as LiftsStore from '../store/Lifts';

// At runtime, Redux will merge together...
type LiftsProps =
    LiftsStore.LiftsState // ... state we've requested from the Redux store
    & typeof LiftsStore.actionCreators // ... plus action creators we've requested
  & RouteComponentProps<{}>; // ... plus incoming routing parameters


class Lifts extends React.PureComponent<LiftsProps> {
  // This method is called when the component is first added to the document
  public componentDidMount() {
    this.ensureDataFetched();
  }

  // This method is called when the route parameters change
  public componentDidUpdate() {
    this.ensureDataFetched();
  }

  public render() {
    return (
      <React.Fragment>
        <h1 id="tabelLabel">Lifts</h1>
        {this.renderLiftsTable()}
      </React.Fragment>
    );
  }

  private ensureDataFetched() {
    this.props.requestLifts();
  }

    private renderLiftsTable() {
    return (
      <table className='table table-striped' aria-labelledby="tabelLabel">
        <thead>
          <tr>
            <th>liftID</th>
            <th>liftName(C)</th>
            <th>isOpen(F)</th>
            <th>seatsAmount</th>
            <th>liftingTime</th>
            <th>queueTime</th>
          </tr>
        </thead>
            <tbody>
                {this.props.lifts.map((lift: LiftsStore.Lift) =>
            <tr key={lift.liftID}>
                        <td>{lift.liftID}</td>
                        <td>{lift.liftName}</td>
                        <td>{lift.isOpen}</td>
                        <td>{lift.seatsAmount}</td>
                        <td>{lift.liftingTime}</td>
                        <td>{lift.queueTime}</td>
            </tr>
          )}
        </tbody>
      </table>
    );
  }

  //private renderPagination() {
  //  const prevStartDateIndex = (this.props.startDateIndex || 0) - 5;
  //  const nextStartDateIndex = (this.props.startDateIndex || 0) + 5;

  //  return (
  //    <div className="d-flex justify-content-between">
  //      <Link className='btn btn-outline-secondary btn-sm' to={`/fetch-data/${prevStartDateIndex}`}>Previous</Link>
  //      {this.props.isLoading && <span>Loading...</span>}
  //      <Link className='btn btn-outline-secondary btn-sm' to={`/fetch-data/${nextStartDateIndex}`}>Next</Link>
  //    </div>
  //  );
  //}
}

export default connect(
  (state: ApplicationState) => state.lifts, // Selects which state properties are merged into the component's props
  LiftsStore.actionCreators // Selects which action creators are merged into the component's props
)(Lifts as any); // eslint-disable-line @typescript-eslint/no-explicit-any
