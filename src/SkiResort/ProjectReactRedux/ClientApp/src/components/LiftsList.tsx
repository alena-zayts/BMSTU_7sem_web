import * as React from 'react';
import { connect } from 'react-redux';
import { RouteComponentProps } from 'react-router';
import { Link } from 'react-router-dom';
import { ApplicationState } from '../store';
import * as LiftsListStore from '../store/__LiftsList';
import MyButton from './UI/button/MyButton';

// At runtime, Redux will merge together...
type LiftsListProps =
    LiftsListStore.LiftsListState // ... state we've requested from the Redux store
    & typeof LiftsListStore.actionCreators // ... plus action creators we've requested
  & RouteComponentProps<{}>; // ... plus incoming routing parameters


class LiftsList extends React.PureComponent<LiftsListProps> {
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
    this.props.requestLiftsList();
  }

    private renderLiftsTable() {
    return (
      <table className='table table-striped' aria-labelledby="tabelLabel">
        <thead>
          <tr>
            <th>liftID</th>
            <th>liftName</th>
            <th>isOpen</th>
            <th>seatsAmount</th>
            <th>liftingTime</th>
            <th>queueTime</th>
          </tr>
        </thead>
            <tbody>
                {this.props.liftsList.map((lift: LiftsListStore.Lift) =>
                    <tr key={lift.LiftID}>
                        <td>{lift.LiftID}</td>
                        <td>{lift.LiftName}</td>
                        <td>{lift.IsOpen}</td>
                        <td>{lift.SeatsAmount}</td>
                        <td>{lift.LiftingTime}</td>
                        <td>{lift.QueueTime}</td>
                        <MyButton onClick={() => this.props.deleteLiftByName(lift.LiftName)}>
                            Удалить
                        </MyButton>
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
  (state: ApplicationState) => state.liftsList, // Selects which state properties are merged into the component's props
  LiftsListStore.actionCreators // Selects which action creators are merged into the component's props
)(LiftsList as any); // eslint-disable-line @typescript-eslint/no-explicit-any
