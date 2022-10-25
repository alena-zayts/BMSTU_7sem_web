////import React from 'react';
////import {TransitionGroup, CSSTransition} from "react-transition-group";
////import LiftItem from './LiftItem';

////const LiftList = ({lifts, title, remove}) => {

////    if (!lifts.length) {
////        return (
////            <h1 style={{textAlign: 'center'}}>
////                Подъемники не найдены!
////            </h1>
////        )
////    }

////    return (
////        <div>
////            <h1 style={{textAlign: 'center'}}>
////                {title}
////            </h1>
////            <TransitionGroup>
////                {lifts.map((lift, index) =>
////                    <CSSTransition
////                        key={lift.liftName}
////                        timeout={500}
////                        classNames="lift"
////                    >
////                        <LiftItem remove={remove} number={index + 1} lift={lift} />
////                    </CSSTransition>
////                )}
////            </TransitionGroup>
////        </div>
////    );
////};

////export default LiftList;