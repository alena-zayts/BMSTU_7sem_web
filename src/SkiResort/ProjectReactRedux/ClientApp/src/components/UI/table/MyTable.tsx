import React, { ReactNode } from 'react';
import * as Styles from '../../../styles/components'
import PropTypes, { InferProps } from "prop-types";
//import classes from './MyButton.module.css';


const MyTable: any = ({ children, ...props }: InferProps<typeof MyTable.propTypes>) => {
    return (
        <table className='table table-striped' aria-labelledby="tabelLabel">
            <thead>
                <tr>
                    {props.headNames.map((headName: string) =>
                        <th>{headName}</th>
                    )}
                </tr>
            </thead>
            <tbody>
                {props.rows.map((row: Array<any>) =>
                    <tr key={row[0]}>
                        {row.map((data: any) =>
                            <td>{data}</td>
                        )}
                    </tr>
                )}
            </tbody>
        </table>
        )
};

MyTable.propTypes = {
    children: PropTypes.oneOfType([
        PropTypes.arrayOf(PropTypes.node),
        PropTypes.node
    ]),

    props: PropTypes.shape(
        {
            onChange: PropTypes.func.isRequired,
            headNames: PropTypes.arrayOf(PropTypes.string),
            rows: PropTypes.arrayOf(PropTypes.arrayOf(PropTypes.any))
        }
    )
};

export default MyTable;